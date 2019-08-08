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
        public DataTemplate ArrayTemplate { get; set; }
        public DataTemplate NotArrayTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var property = (item as SystemObjectPropertyValueModel);

            switch (property?.TypeID)
            {
                case null: return GroupTemplate;
                case 1: return BoolTemplate;
                case 2: return DateTimeTemplate;
                case 3: return FloatTemplate;
                case 4: return IntTemplate;
                case 5: {
                        if (property.Array == true)
                        {
                            return ArrayTemplate;
                        }
                        else return NotArrayTemplate;
                    }
                case 6: return StringTemplate;
                case 7: return HumanTemplate;
                    default: return ObjectTemplate;
            }
        }
    }
}
