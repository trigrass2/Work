﻿using System;
using System.Collections.Generic;
using System.Text;
using Vertical.Models;
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

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {            
            switch ((item as SystemObjectTypePropertyModel).PropertyID)
            {
                case 1: return BoolTemplate;
                case 2: return DateTimeTemplate;
                case 3: return FloatTemplate;
                case 4: return IntTemplate;
                case 5: return ObjectTemplate;
                case 6: return StringTemplate;
                case 7: return HumanTemplate;
                    default: return ObjectTemplate;
            }

           // return ((Person)item).DateOfBirth.Year >= 1980 ? ValidTemplate : InvalidTemplate;
        }
    }
}
