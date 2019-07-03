using Android.Util;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;
using static Vertical.Constants;

namespace Vertical.Services
{
    public static class Api
    {     
        public static AccessToken Token = new AccessToken();

        /// <summary>
        /// Токен для авторизации
        /// </summary>
        public static string AccessToken { get; private set; }

        /// <summary>
        /// Имена методов Api
        /// </summary>
        public enum NameMetodsApi
        {
            AddSystemObject,
            GetSystemObjectTypes,
            GetSystemObjects,
            EditSystemObjectModel
        }

        /// <summary>
        /// Добавляет новый объект в систему
        /// </summary>
        /// <typeparam name="T">тип добавляемого объекта</typeparam>
        /// <param name="model">добавляемый объект</param>
        public static bool SendData<T>(NameMetodsApi nameMetod ,T model = default(T))
        {
            try
            {
                var client = new RestClient(domain + $"/api/{Enum.GetName(typeof(NameMetodsApi), nameMetod)}");

                var request = new RestRequest(Method.POST);
                request.AddHeader("authorization", $"Bearer {AccessToken}");

                var jsonModel = JsonConvert.SerializeObject(model);

                request.AddParameter("application/json", jsonModel, ParameterType.RequestBody);

                var restResponse = client.Execute(request);
                if (restResponse.IsSuccessful)
                {
                    Log.WriteLine(LogPriority.Info, $"{nameof(SendData)}", "Данные отправлены");
                    return true;
                }
                else
                {
                    Log.WriteLine(LogPriority.Info, $"{nameof(SendData)}", $"Данные не отправлены -> {restResponse.Content}");
                    return false;
                }

            }
            catch (Exception ex)
            {
                Log.WriteLine(LogPriority.Error, $"{nameof(GetServerData)}", ex.Message);
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
        public  static IList<T> GetServerData<T>(NameMetodsApi nameMetod, object model = default(object), [CallerMemberName]string callingFunction = "")
        {
            try
            {
                var client = new RestClient(domain + $"/api/{Enum.GetName(typeof(NameMetodsApi), nameMetod)}");

                var request = new RestRequest(Method.POST);
                request.AddHeader("authorization", $"Bearer {AccessToken}");
                request.AddParameter("application/json", JsonConvert.SerializeObject(model), ParameterType.RequestBody);

                var restResponse = client.Execute(request);
                if (restResponse.IsSuccessful)
                {
                    Log.WriteLine(LogPriority.Info, $"{nameof(SendData)}", "Данные получены");
                    return JsonConvert.DeserializeObject<IList<T>>(restResponse.Content);
                }
                else
                {
                    Log.WriteLine(LogPriority.Info, $"{nameof(SendData)}", $"Данные не получены -> {restResponse.Content}");
                    return default(IList<T>);
                }
                
            }
            catch (Exception ex)
            {
                Log.WriteLine(LogPriority.Error, $"{nameof(GetServerData)}", ex.Message);
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
                    Token = JsonConvert.DeserializeObject<AccessToken>(restResponse.Content);
                    AccessToken = Token.Access_token;
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
    }

    public class AccessToken
    {
        public string Access_token { get; set; }
    }
}
