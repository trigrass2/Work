using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Vertical.CustomViews
{
    public class GroupingModel<T> : ObservableCollection<T>, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Name { get; set; }

        public GroupingModel(string name)
        {
            Name = name == default(string) ? "Без группы" : name;
        }

    }
}
