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
