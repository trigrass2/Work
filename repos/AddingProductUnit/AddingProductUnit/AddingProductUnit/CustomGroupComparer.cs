using Syncfusion.DataSource;
using Syncfusion.DataSource.Extensions;
using System;
using System.Collections.Generic;
//using System.ComponentModel;
using System.Text;

namespace AddingProductUnit
{
    public class CustomGroupComparer : IComparer<GroupResult>, ISortDirection
    {
        public CustomGroupComparer()
        {
            this.SortDirection = ListSortDirection.Ascending;
        }

        public ListSortDirection SortDirection
        {
            get;
            set;
        }

        public int Compare(GroupResult x, GroupResult y)
        {
            int groupX;
            int groupY;

            groupX = x.Count;
            groupY = y.Count;

            // Objects are compared and return the SortDirection
            if (groupX.CompareTo(groupY) > 0)
                return SortDirection == ListSortDirection.Ascending ? 1 : -1;
            else if (groupX.CompareTo(groupY) == -1)
                return SortDirection == ListSortDirection.Ascending ? -1 : 1;
            else
                return 0;
        }
    }
}
