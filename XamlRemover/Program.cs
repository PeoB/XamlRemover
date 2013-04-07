using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace XamlRemover
{
    class Program
    {
        private static string _removeAttr;

        static void Main(string[] args)
        {
            var files = Directory.EnumerateFiles(args[0], "*.xaml", SearchOption.TopDirectoryOnly).ToList();
            
            files.ToList()
                .ForEach(TransformAndSaveXml);
        }

        private static void TransformAndSaveXml(string obj)
        {
            var doc = XDocument.Load(obj);
            var namespaces = doc.Root.Attributes().Where(a => a.IsNamespaceDeclaration).ToDictionary(a=>a.Name.LocalName,a=>a.Value);
            var nodes = doc.Descendants();
            _removeAttr = "{http://schemas.peosupersajts.com/xamlmorph}Remove";
            var toRemove=nodes.SelectMany(el => el.Attributes()).Where(attr => attr.Name == _removeAttr).ToList()
                .Select(attr=>new {Element=attr.Parent,ToRemove=attr.Value.Split(' ').Concat(new []{_removeAttr})})
                .ToList();

            toRemove.SelectMany(n => SuitingAttrs(n.Element, n.ToRemove, namespaces))
                .ToList()
                .ForEach(attr=>attr.Remove());
            toRemove.SelectMany(n => SuitingElements(n.Element, n.ToRemove,namespaces))
                .ToList().ForEach(el=>el.Remove());
            doc.Save(obj);
        }

        private static IEnumerable<XElement> SuitingElements(XElement element, IEnumerable<string> toRemove, Dictionary<string, string> namespaces)
        {
            var toRemoveList = toRemove.Select(r=>FullElementName(element,r,namespaces)).ToList();
            if (toRemoveList.Contains("this"))
                return new[] {element};
            return
            element.Elements()
                   .Where(el => toRemoveList.Any(r => el.Name==r));

        }

        private static string FullElementName(XElement element, string name, Dictionary<string, string> namespaces)
        {
            if (name.Contains("{")) return name;
            if (name == "this") return name;
            if (!name.Contains(":"))
                return element.Name+"."+name;
            var ns = namespaces[name.Split(':').First()];
            return string.Format("{{{0}}}{1}.{2}",ns,element.Name.LocalName,name.Split(':').Last());
        }

        private static IEnumerable<XAttribute> SuitingAttrs(XElement element, IEnumerable<string> toRemove,Dictionary<string,string> namespaces)
        {
            toRemove=toRemove.Select(r => FullAttrName(r, namespaces));
            return
                element.Attributes()
                       .Where(attr => toRemove.Any(r => attr.Name.ToString()==r));
        }

        private static string FullAttrName(string attr, IReadOnlyDictionary<string, string> namespaces)
        {
            if (attr.Contains("{") || !attr.Contains(":"))
                return attr;
            return "{"+namespaces[attr.Split(':').First()]+"}" + attr.Split(':').Last();
        }
    }
}
