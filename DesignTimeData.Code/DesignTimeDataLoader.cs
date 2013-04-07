using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using XamlRemover.Extensions;

namespace DesignTimeData.Code
{
    public class DesignTimeDataLoader
    {
        public IDictionary<Type, Func<object>> Creators = new Dictionary<Type, Func<object>>
            {
                {typeof(string),()=>"Some string"},
                {typeof(int),()=>13}
            };
        public object Populate(object obj)
        {
            Creators[obj.GetType()] = () => obj;
            var add = obj.GetType().GetRuntimeMethods().FirstOrDefault(m => m.Name == "Add" && m.GetParameters().Count()==1);
            if (add != null)
                return PopulateCollection(obj, add);

            obj.GetType().GetRuntimeProperties()
               .Where(p => p.SetMethod.IsPublic)
               .ForEach(p => p.SetMethod.Invoke(obj, ValueFor(p.PropertyType)));

            return obj;
        }

        private object PopulateCollection(object o, MethodInfo add)
        {
            Enumerable.Range(0, 3).ForEach(
                i =>
                    add.Invoke(o, ValueFor(add.GetParameters().First().ParameterType))
                );
            return o;
        }

        private object[] ValueFor(Type propertyType)
        {
            try
            {
                return
                    Creators.ContainsKey(propertyType)
                    ? new[] { Creators[propertyType]() }
                    : new[] { Populate(Activator.CreateInstance(propertyType)) };
            }
            catch
            {
                return new object[] { null };
            }
        }
    }
}
