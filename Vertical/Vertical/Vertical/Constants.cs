using System;
using System.Collections.Generic;
using System.Text;

namespace Vertical
{
    public static class Constants
    {
        /// <summary>
        /// адрес для api
        /// </summary>
        public static readonly string domain = "https://obt.pik-industry.ru:444";
        public static readonly string AndroidSecret = "android=3ef5a6e3-874d-44b7-a40f-c1410aadb92d";
        public static readonly string IOSSecret = "ios=302f53f8-36cb-4f7f-8ba7-40b98efd67c4;";
        //public enum IsAddOrEdit
        //{
        //    Add,
        //    Edit
        //}

        /// <summary>
        /// Состояние экрана
        /// </summary>
        public enum States
        {
            Loading,
            Normal,
            Error,
            NoInternet,
            NoData,
            NoAccess
        }

    }
}
