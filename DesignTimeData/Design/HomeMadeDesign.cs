using System.Dynamic;
using Windows.UI.Xaml;

namespace DesignTimeData.Design
{
    public class HomeMadeDesign:DynamicObject
    {
        public static readonly DependencyProperty DataContextProperty = DependencyProperty.RegisterAttached("DataContext", typeof (object), typeof (HomeMadeDesign), new PropertyMetadata(default(object)));
        public static readonly DependencyProperty RemoveProperty = DependencyProperty.RegisterAttached("Remove", typeof (string), typeof (HomeMadeDesign), new PropertyMetadata(default(string)));

        public static object GetDataContext(UIElement element)
        {
            return (object) element.GetValue(DataContextProperty);
        }

        public static void SetDataContext(UIElement element, object value)
        {
            var el=(FrameworkElement) element;
            el.DataContext = new {Test = "Tomte"};
            element.SetValue(DataContextProperty, value);
        }

        public string Test { get; set; }

        public static string GetRemove(UIElement element)
        {
            return (string) element.GetValue(RemoveProperty);
        }

        public static void SetRemove(UIElement element, string value)
        {
            element.SetValue(RemoveProperty, value);
        }
    }
}