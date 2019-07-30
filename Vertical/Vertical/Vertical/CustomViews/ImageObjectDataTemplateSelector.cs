using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vertical.Models;
using Vertical.Services;
using Xamarin.Forms;

namespace Vertical.CustomViews
{
    public class ImageObjectDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate CheckListDataTemplate { get; set; }
        public DataTemplate FolderDataTemplate { get; set; }
        public DataTemplate ObjectDataTemplate { get; set; }

        IList<SystemObjectTypeModel> types = Api.GetDataFromServer<SystemObjectTypeModel>("System/GetSystemObjectTypes", new { ShowHidden = true});
                          
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {            
            var prototypeId = types.Where(x => x.ID == (item as SystemObjectModel)?.TypeID).Select(p => p.PrototypeID);
            switch (prototypeId.FirstOrDefault())
            {
                case 1: return FolderDataTemplate;
                case 2: return CheckListDataTemplate;                
                    default: return ObjectDataTemplate;
            }
        }
    }
}
