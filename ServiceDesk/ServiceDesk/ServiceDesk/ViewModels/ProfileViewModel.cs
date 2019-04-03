using ServiceDesk.PikApi;
using ServiceDesk.Models;
using Xamarin.Forms;
using System.ComponentModel;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Android.Webkit;
using ServiceDesk.Views;

namespace ServiceDesk.ViewModels
{
    public class ProfileViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
       
        public UserModel MyProfile { get; set; } 
        public ObservableCollection<ServiceDesk_GroupUserListView> Groups { get; set; }

        public INavigation Navigation { get; set; }
        public ICommand ExitCommand { get; set; }

        public ProfileViewModel()
        {
            Groups = new ObservableCollection<ServiceDesk_GroupUserListView>();
            UpdateUser();
            ExitCommand = new Command(Exit);
        }

        /// <summary>
        /// Выводит из текущего профиля
        /// </summary>
        private void Exit()
        {
            //var cookieManager = CookieManager.Instance;
            //cookieManager.RemoveAllCookie();
            App.Current.MainPage = new NavigationPage(new StartPage());
        }

        /// <summary>
        /// Обновляет список пользователей
        /// </summary>
        private void UpdateUser()
        {
            var User = ServiceDeskApi.GetUser<ApplicationUser>(ServiceDeskApi.ApiEnum.GetUserInfo);
            MyProfile = ServiceDeskApi.GetAllUsers(new { User_id = User.Id, Search = default(string) }, ServiceDeskApi.ApiEnum.GetUsersList).FirstOrDefault();
            Groups.Clear();
            var groupCollection = ServiceDeskApi.GetDataServisDeskManagment<ServiceDesk_GroupUserListView>(new { Group_id = default(int?), User_id = User.Id }, ServiceDeskApi.ApiEnum.GetGroupsUsers);

            foreach(var g in groupCollection)
            {
                Groups.Add(g);
            }
        }
    }
}
