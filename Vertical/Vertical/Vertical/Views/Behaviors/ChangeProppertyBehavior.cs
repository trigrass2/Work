using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Vertical.Views.Behaviors
{
    public class ChangeProppertyBehavior : Behavior<Entry>
    {
        //public static readonly BindableProperty CommandProperty = BindableProperty.CreateReadOnly(
        //    "Command",
        //    typeof(ICommand),
        //    typeof(ChangeProppertyBehavior)

        //    );

        //public ICommand Command
        //{
        //    get { return (ICommand)base.GetValue(CommandProperty); }
        //    set { base.SetValue(CommandProperty, value); }
        //}

        protected override void OnAttachedTo(Entry entry)
        {
            entry.TextChanged += OnEntryTextChanged;
            base.OnAttachedTo(entry);
        }

        protected override void OnDetachingFrom(Entry entry)
        {
            entry.TextChanged -= OnEntryTextChanged;
            base.OnDetachingFrom(entry);
        }

        private void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        {
            double result;
            bool isValid = double.TryParse(args.NewTextValue, out result);
            ((Entry)sender).TextColor = isValid ? Color.Default : Color.Red;
        }
    }
}