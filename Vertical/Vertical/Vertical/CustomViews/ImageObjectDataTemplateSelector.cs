using System;
using System.Collections.Generic;
using System.Text;
using Vertical.Models;
using Xamarin.Forms;

namespace Vertical.CustomViews
{
    public class ImageObjectDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate CheckListDataTemplate { get; set; }
        public DataTemplate FolderDataTemplate { get; set; }
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var id = (item as SystemObjectModel)?.TypeID;

            switch (id)
            {  
                case 2: return CheckListDataTemplate;
                
                default: return FolderDataTemplate;
            }
        }
    }
}
