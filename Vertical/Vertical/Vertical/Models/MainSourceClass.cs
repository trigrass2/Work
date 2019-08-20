
using System.Collections.ObjectModel;

namespace Vertical.Models
{
    public class MainSourceClass : SystemObjectPropertyValueModel
    {
        public ObservableCollection<string> ArrayValue { get; set; }

        public MainSourceClass(SystemObjectPropertyValueModel valueModel)
        {
            if(valueModel != null)
            {
                ID = valueModel.ID;
                Num = valueModel.Num;
                Name = valueModel.Name;
                GroupName = valueModel.GroupName;
                Value = valueModel.Value;
                ValueNum = valueModel.ValueNum;
                SystemObjectGUID = valueModel.SystemObjectGUID;
                SystemObjectName = valueModel.SystemObjectName;
                Timestamp = valueModel.Timestamp;
                UserGUID = valueModel.UserGUID;
                UserName = valueModel.UserName;
                GroupName = valueModel.GroupName;
                GroupID = valueModel.GroupID;
                TypeID = valueModel.TypeID;
                TypeName = valueModel.TypeName;
                SourceObjectParentGUID = valueModel.SourceObjectParentGUID;
                SourceObjectTypeID = valueModel.SourceObjectTypeID;
                Locked = valueModel.Locked;
                Array = valueModel.Array;
            }
        }
    }
}
