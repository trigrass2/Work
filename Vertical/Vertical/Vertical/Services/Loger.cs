using Android.Util;
using RestSharp;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Vertical.Services
{
    public static class Loger
    {
        public async static Task WriteMessageAsync(LogPriority logPriority, string text = default(string), string errorMessage = default(string), [CallerMemberName] string invokeMetodName = "")
        {
            if(logPriority == LogPriority.Error)
            {
                await SendError(text, errorMessage, invokeMetodName);
            }           
            
            Log.WriteLine(logPriority, $"In {invokeMetodName}", $"{errorMessage}");
        }

        private async static Task SendError(string textMessage, string error, string invokeMethod)
        {
            try
            {
                string textMsg = $"In {invokeMethod} -> {textMessage} -> Error: {error}";
                RestClient client = new RestClient($"https://api.telegram.org/bot870858359:AAH0xAUXEm3zNVVFM7buY6Avwvrj_av4Rac/sendMessage?chat_id=-1001483917651&text={textMsg}")
                {
                    Timeout = 5000
                };
                RestRequest restRequest = new RestRequest(Method.POST);

                var responce = await client.ExecuteTaskAsync(restRequest);
            }
            catch (Exception ex)
            {                        
                WriteMessageAsync(LogPriority.Error, "Error in send", ex.Message);
            }

        }
    }
}
