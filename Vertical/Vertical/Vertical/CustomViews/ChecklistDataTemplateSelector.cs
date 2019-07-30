using System.Linq;
using Vertical.Models;
using Vertical.Services;
using Xamarin.Forms;

namespace Vertical.CustomViews
{
    public class ChecklistDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate BoolTemplate { get; set; }
        public DataTemplate DateTimeTemplate { get; set; }
        public DataTemplate FloatTemplate { get; set; }
        public DataTemplate IntTemplate { get; set; }
        public DataTemplate ObjectTemplate { get; set; }
        public DataTemplate StringTemplate { get; set; }
        public DataTemplate HumanTemplate { get; set; }
        public DataTemplate GroupTemplate { get; set; }
        public DataTemplate ArrangementDataTemplate { get; set; }
        public DataTemplate PostDataTemplate { get; set; }
        public DataTemplate HumanWithProfiDataTemplate { get; set; }
        public DataTemplate ProfiDataTemplate { get; set; }
        public DataTemplate CraneDataTemplate { get; set; }
        public DataTemplate CatalogDataTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var id = (item as SystemObjectPropertyValueModel)?.TypeID;

            switch (id)
            {
                case null: return GroupTemplate;
                case 1: return BoolTemplate;
                case 2: return DateTimeTemplate;
                case 3: return FloatTemplate;
                case 4: return IntTemplate;
                case 5:
                    {
                        var objectType = Api.GetDataFromServer<SystemObjectModel>("System/GetSystemObjects", new { ObjectGUID = (item as SystemObjectPropertyValueModel).Value });
                        
                        switch (objectType?.FirstOrDefault().TypeID)
                        {
                            case 1: return CatalogDataTemplate;
                            case 3: return ArrangementDataTemplate;
                            case 4: return HumanWithProfiDataTemplate;
                            case 5: return PostDataTemplate;
                            case 6: return ProfiDataTemplate;
                            case 9: return CraneDataTemplate;
                                default: return ObjectTemplate;
                        }
                        
                    }
                case 6: return StringTemplate;
                case 7: return HumanTemplate;
                    default: return ObjectTemplate;
            }
        }
    }
}
