using System;
using System.Collections.Generic;
using System.Text;
using ServiceDesk.PikApi;
using ServiceDesk.Models;

namespace ServiceDesk.ViewModels
{
    public class ProfileViewModel
    {
        private ApplicationUser _user;

        public ProfileViewModel()
        {
            UpdateUser();
        }

        private async void UpdateUser()
        {
            _user = ServiceDeskApi.GetUser<ApplicationUser>(ServiceDeskApi.ApiEnum.GetUserInfo);
            var myProfile = await ServiceDeskApi.GetAllUsersAsync(new { User_id = default(string), Search = default(string) }, ServiceDeskApi.ApiEnum.GetUsersList);
        }
    }
}
