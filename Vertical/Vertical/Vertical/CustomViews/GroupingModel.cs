using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Vertical.CustomViews
{
    public class GroupingModel<T> : ObservableCollection<T>
    {
        public string Name { get; set; }

        public GroupingModel(string name)
        {
            Name = name == default(string) ? "Без группы" : name;
        }

        public static GroupingModel<T> GetGroup(string nameGroup, IList<T> items)
        {
            GroupingModel<T> groupProperties = new GroupingModel<T>(nameGroup);

            foreach (var i in items.Where(x => x.GroupName == nameGroup))
            {
                groupProperties.Add(i);
            }
            return groupProperties;
        }
    }
}
