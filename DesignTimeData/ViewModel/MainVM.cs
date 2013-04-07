using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace DesignTimeData.ViewModel
{
    public class MainVM:INotifyPropertyChanged
    {
        public string Test { get; set; } 
        public ObservableCollection<string> Test2 { get; set; }
        public Dictionary<string, string> Strings { get; set; }
        public ObservableCollection<MainVM> VMs { get; set; }
        public float f { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

    }
}