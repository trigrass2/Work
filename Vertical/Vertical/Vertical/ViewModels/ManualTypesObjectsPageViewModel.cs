using System.Collections.ObjectModel;
using System.ComponentModel;
using Vertical.Models;
using Vertical.Services;
using Vertical.Views;
using Xamarin.Forms;
using static Vertical.Constants;

namespace Vertical.ViewModels
{
    public class ManualTypesObjectsPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<SystemObjectTypeModel> SystemObjectTypesModels { get; set; }
        private SystemObjectTypeModel _selectedObjectTypeModel;
        public SystemObjectTypeModel SelectedObjectTypeModel
        {
            get
            {
                return _selectedObjectTypeModel;
            }
            set
            {
                var temp = value.ID;
                _selectedObjectTypeModel = null;
                OpenInfoPage(temp);                               
            }
        }

        public INavigation Navigation { get; set; }
        public States States { get; set; } = States.Normal;

        public ManualTypesObjectsPageViewModel()
        {
            SystemObjectTypesModels = new ObservableCollection<SystemObjectTypeModel>();
            UpdateSystemObjectTypesModels();
        }

        private void UpdateSystemObjectTypesModels()
        {
            SystemObjectTypesModels.Clear();

            foreach(var t in Api.GetDataFromServer<SystemObjectTypeModel>("System/GetSystemObjectTypes"))
            {
                SystemObjectTypesModels.Add(t);
            }
        }

        private async void OpenInfoPage(int id)
        {
            await Navigation.PushModalAsync(new TypeModelInfoPage(id));
        }
    }
}
