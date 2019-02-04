using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassengersCounter.Model
{
    public class SelectableItemWrapper<T>
    {
        public bool IsSelected { get; set; }
        public T Item { get; set; }
    }
}
