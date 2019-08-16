using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using Microsoft.CSharp;
using Microsoft.CSharp.RuntimeBinder;

namespace Vertical.Models
{
    public class MainSourceClass : SystemObjectPropertyValueModel
    {
        public object ArrayValue { get; set; }

        public MainSourceClass(SystemObjectPropertyValueModel valueModel)
        {
            ID = valueModel.ID;
            Name = valueModel.Name;
            GroupName = valueModel.GroupName;
        }
    }
}
