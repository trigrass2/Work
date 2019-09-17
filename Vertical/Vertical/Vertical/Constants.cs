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
        public enum IsAddOrEdit
        {
            Add,
            Edit
        }

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
