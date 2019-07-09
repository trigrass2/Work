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
        public static readonly string domain = "http://34.90.63.119";

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
