using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace Vertical
{
    public class Grouping<TKey, TElement> : IGrouping<TKey, TElement>, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly TKey key;
        public ObservableCollection<TElement> Values { get; set; }

        public Grouping(TKey key, IEnumerable<TElement> values)
        {
            if (values == null)
                throw new ArgumentNullException("values");
            this.key = key;
            this.Values = new ObservableCollection<TElement>(values);
        }

        public TKey Key
        {
            get { return key; }
        }

        public IEnumerator<TElement> GetEnumerator()
        {
            return Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
