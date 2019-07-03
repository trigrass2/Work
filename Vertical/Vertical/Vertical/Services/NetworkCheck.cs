using Plugin.Connectivity;
using Xamarin.Forms;

namespace Vertical.Services
{
    public class NetworkCheck
    {
        /// <summary>
        /// Проверяет соединение с интернетом
        /// </summary>
        /// <returns></returns>
        public static bool IsInternet()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                return true;
            }
            else
            {
                
                return false;
            }
        }
    }
}
