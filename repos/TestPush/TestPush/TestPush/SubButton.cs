using System;
using System.Collections.Generic;
using System.Text;

namespace TestPush
{
    public class SubButton 
    {
        public int Id { get; set; }

        public string Label { get; set; }

        public SubButton(int id, string label)
        {
            Id = id;
            Label = label;
        }

    }
}
