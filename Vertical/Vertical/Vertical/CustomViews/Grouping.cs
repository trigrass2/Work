using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Vertical.CustomViews
{
    public class Grouping<T> : ObservableCollection<T>
    {
        public string Name { get; private set; }
        public int? ID { get; private set; }
        public Grouping(string name, IEnumerable<T> items)
        {

            Name = name == default ? "Без группы" : name;
            foreach (T item in items)
                Items.Add(item);
        }
    }
}
