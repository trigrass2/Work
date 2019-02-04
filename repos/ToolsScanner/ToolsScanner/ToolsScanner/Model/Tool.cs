using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using GalaSoft.MvvmLight;

namespace ToolsScanner.Model
{
  
    public class Tool : ViewModelBase
    {
        private string tool_id;
        private string tool_name;
        private string person_id;
        private string person_name;

        public string Tool_id
        {
            get { return tool_id; }
            set { tool_id = value; RaisePropertyChanged();/*OnPropertyChanged("Tool_id");*/ }
        }

        public string Tool_name
        {
            get { return tool_name; }
            set { tool_name = value; RaisePropertyChanged();/*OnPropertyChanged("Tool_name");*/ }
        }

        public string Person_id
        {
            get { return person_id; }
            set { person_id = value; RaisePropertyChanged();/*OnPropertyChanged("Person_id");*/ }
        }

        public string Person_name
        {
            get { return person_name; }
            set { person_name = value; RaisePropertyChanged();/*OnPropertyChanged("Person_name");*/ }
        }        
    }
}
