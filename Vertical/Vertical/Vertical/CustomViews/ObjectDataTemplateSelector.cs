using System.Linq;
using Vertical.Models;
using Vertical.Services;
using Xamarin.Forms;

namespace Vertical.CustomViews
{
    public class ObjectDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ArrangementDataTemplate { get; set; }
        public DataTemplate PostDataTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {

            var objectType = Api.GetDataFromServer<SystemObjectModel>("System/GetSystemObjects", new { ObjectGUID  = (item as SystemObjectPropertyValueModel).Value});

            switch (objectType.FirstOrDefault().TypeID)
            {
                case 3: return ArrangementDataTemplate;
                    default : return PostDataTemplate;
            }
        }
    }
}
