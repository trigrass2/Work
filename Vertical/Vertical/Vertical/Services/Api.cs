using Android.Util;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using static Vertical.Constants;

namespace Vertical.Services
{
    /// <summary>
    /// Класс для работы с API
    /// </summary>
    public static class Api
    {           
        /// <summary>
        /// Токен для авторизации
        /// </summary>
        public static string AccessToken { get; private set; }

        /// <summary>
        /// Добавляет новый объект в систему
        /// </summary>
        /// <typeparam name="T">тип добавляемого объекта</typeparam>
        /// <param name="model">добавляемый объект</param>
        public static bool SendDataToServer<T>(string nameMetod, T model = default(T))
        {            
            try
            {
                var client = new RestClient(domain + $"/api/{nameMetod}");

                var request = new RestRequest(Method.POST);
                request.AddHeader("authorization", $"Bearer {AccessToken}");

                var jsonModel = JsonConvert.SerializeObject(model);

                request.AddParameter("application/json", jsonModel, ParameterType.RequestBody);

                var restResponse = client.Execute(request);
                if (restResponse.IsSuccessful)
                {
                    Log.WriteLine(LogPriority.Info, $"{nameof(SendDataToServer)}", "Данные отправлены");
                    return true;
                }
                else
                {
                    Log.WriteLine(LogPriority.Info, $"{nameof(SendDataToServer)}", $"Данные не отправлены -> {restResponse.Content}");
                    return false;
                }

            }
            catch (Exception ex)
            {
                Log.WriteLine(LogPriority.Error, $"{nameof(SendDataToServer)}", ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Добавляет новый объект в систему
        /// </summary>
        /// <typeparam name="T">тип добавляемого объекта</typeparam>
        /// <param name="model">добавляемый объект</param>
        public static async Task<bool> SendDataToServerAsync<T>(string nameMetod, T model = default(T))
        {
            try
            {
                var client = new RestClient(domain + $"/api/{nameMetod}");

                var request = new RestRequest(Method.POST);
                request.AddHeader("authorization", $"Bearer {AccessToken}");

                var jsonModel = JsonConvert.SerializeObject(model);

                request.AddParameter("application/json", jsonModel, ParameterType.RequestBody);

                var restResponse = await client.ExecuteTaskAsync(request);
                if (restResponse.IsSuccessful)
                {
                    Log.WriteLine(LogPriority.Info, $"{nameof(SendDataToServerAsync)}", "Данные отправлены");
                    return true;
                }
                else
                {
                    Log.WriteLine(LogPriority.Info, $"{nameof(SendDataToServerAsync)}", $"Данные не отправлены -> {restResponse.Content}");
                    return false;
                }

            }
            catch (Exception ex)
            {
                Log.WriteLine(LogPriority.Error, $"{nameof(SendDataToServerAsync)}", ex.Message);
                return false;
            }
        }


        /// <summary>
        /// Возвращает данные с сервера
        /// </summary>
        /// <typeparam name="T">тип возвращаемых данных</typeparam>
        /// <param name="model">параметры запроса</param>
        /// <param name="nameMetod">имя метода</param>
        /// <param name="callingFunction">вызывающая функция</param>
        /// <returns>коллекцтя объектов</returns>
        public static T[] GetDataFromServer<T>(string nameMetod, object model = default(object))
        {
            try
            {
                var client = new RestClient(domain + $"/api/{nameMetod}");

                var request = new RestRequest(Method.POST);
                request.AddHeader("authorization", $"Bearer {AccessToken}");
                request.AddParameter("application/json", JsonConvert.SerializeObject(model), ParameterType.RequestBody);

                var restResponse = client.Execute(request);
                if (restResponse.IsSuccessful)
                {
                    Log.WriteLine(LogPriority.Info, $"{nameof(GetDataFromServer)}", "Данные получены");
                    return JsonConvert.DeserializeObject<T[]>(restResponse.Content);
                }
                else
                {
                    Log.WriteLine(LogPriority.Info, $"{nameof(GetDataFromServer)}", $"Данные не получены -> {restResponse.Content}");
                    return default(T[]);
                }
                
            }
            catch (Exception ex)
            {
                Log.WriteLine(LogPriority.Error, $"{nameof(GetDataFromServer)}", ex.Message);
                return default(T[]);
            }                    
        }

        /// <summary>
        /// Возвращает данные с сервера
        /// </summary>
        /// <typeparam name="T">тип возвращаемых данных</typeparam>
        /// <param name="model">параметры запроса</param>
        /// <param name="nameMetod">имя метода</param>
        /// <param name="callingFunction">вызывающая функция</param>
        /// <returns>коллекцтя объектов</returns>
        public async static Task<IList<T>> GetDataFromServerAsync<T>(string nameMetod, object model = default(object))
        {
            try
            {
                var client = new RestClient(domain + $"/api/{nameMetod}");

                var request = new RestRequest(Method.POST);
                request.AddHeader("authorization", $"Bearer {AccessToken}");
                request.AddParameter("application/json", JsonConvert.SerializeObject(model), ParameterType.RequestBody);

                var restResponse = await client.ExecuteTaskAsync(request);

                if (restResponse.IsSuccessful)
                {
                    Log.WriteLine(LogPriority.Info, $"{nameof(GetDataFromServerAsync)}", "Данные получены");
                    return JsonConvert.DeserializeObject<IList<T>>(restResponse.Content);
                }
                else
                {
                    Log.WriteLine(LogPriority.Info, $"{nameof(GetDataFromServerAsync)}", $"Данные не получены -> {restResponse.Content}");
                    return default(IList<T>);
                }

            }
            catch (Exception ex)
            {
                Log.WriteLine(LogPriority.Error, $"{nameof(GetDataFromServerAsync)}", ex.Message);
                return default(IList<T>);
            }
        }

        /// <summary>
        /// Возвращает токен для api
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static HttpStatusCode GetToken(string login, string password)
        {
            try
            {                
                var client = new RestClient(domain + "/Token")
                {
                    Timeout = 5000
                };

                var request = new RestRequest(Method.POST);
                request.AddParameter("grant_type", "password");
                request.AddParameter("username", login);
                request.AddParameter("password", password);
                var restResponse = client.Execute(request);
                if (restResponse.IsSuccessful)
                {
                    AccessToken = JsonConvert.DeserializeObject<AccessToken>(restResponse.Content).Access_token;                    
                }

                Log.WriteLine(LogPriority.Info, $"{nameof(GetToken)}", "Авторизация прошла успешно");
                return restResponse.StatusCode;
            }
            catch (Exception ex)
            {
                Log.WriteLine(LogPriority.Error, $"{nameof(GetToken)}", ex.Message);
                return default(HttpStatusCode);
            }
            
        }

        /// <summary>
        /// Регистрация аккаунта
        /// </summary>
        /// <param name="login">логин</param>
        /// <param name="password">пароль</param>
        /// <param name="confirmPassword">подтверждение пароля</param>
        public static void RegisterAccount(string login, string password, string confirmPassword)
        {
            var client = new RestClient("http://34.90.63.119/api/Account/Register");

            var request = new RestRequest(Method.POST);

            request.AddParameter("Email", login);
            request.AddParameter("Password", password);
            request.AddParameter("ConfirmPassword", confirmPassword);

            var restResponse = client.Execute(request);
        }

        public static void SendError(string textMessage, [CallerMemberName] string invokeMetodName = "")
        {
            try
            {
                string textMsg = $"In {invokeMetodName} {textMessage}";
                RestClient client = new RestClient($"https://api.telegram.org/bot870858359:AAH0xAUXEm3zNVVFM7buY6Avwvrj_av4Rac/sendMessage?chat_id=@v_error&text={textMsg}");
                RestRequest restRequest = new RestRequest(Method.POST);

                var responce = client.Execute(restRequest);
            }
            catch (Exception ex)
            {
                Log.WriteLine(LogPriority.Error, $"{nameof(SendError)} in {invokeMetodName}", $"{ex.Message}");
            }

        }
    }

    public class AccessToken
    {
        public string Access_token { get; set; }
    }
}
