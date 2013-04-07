using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace DesignTimeData.Design
{
    public class Selector:DataTemplate
    {
        public static readonly DependencyProperty DataProperty = DependencyProperty.Register("Data", typeof (object), typeof (Selector), new PropertyMetadata(default(object),OnPropertyChangedCallback));
        

        private static void OnPropertyChangedCallback(DependencyObject o, DependencyPropertyChangedEventArgs args)
        {
            var x=(Selector) o;
        }

        public DataTemplate X { get; set; }
        public DataTemplate Y { get; set; }
        public object Template
        {
            get { return X; }
            set { }
        }

        public void SelectTemplate(object o)
        {
            
        }
        public object Data
        {
            get { return (object) GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }
    }
}