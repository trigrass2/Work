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
            var client = new RestClient(domain + $"/api/{nameMetod}");

            try
            {
                

                var request = new RestRequest(Method.POST);
                request.AddHeader("authorization", $"Bearer {AccessToken}");

                var jsonModel = JsonConvert.SerializeObject(model);

                request.AddParameter("application/json", jsonModel, ParameterType.RequestBody);

                var restResponse = client.Execute(request);
                if (restResponse.IsSuccessful)
                {
                    Loger.WriteMessage(LogPriority.Info, $"В запросе {client?.BaseUrl}: Данные отправлены");
                    return true;
                }
                else
                {
                    Loger.WriteMessage(LogPriority.Info, $"В запросе {client?.BaseUrl}: Данные не отправлены -> {restResponse.Content}");
                    return false;
                }

            }
            catch (Exception ex)
            {
                Loger.WriteMessage(LogPriority.Error, $"В запросе {client?.BaseUrl} Ошибка при отправке данных на сервер", ex.Message);
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
            var client = new RestClient(domain + $"/api/{nameMetod}");

            try
            {
                var request = new RestRequest(Method.POST);
                request.AddHeader("authorization", $"Bearer {AccessToken}");

                var jsonModel = JsonConvert.SerializeObject(model);

                request.AddParameter("application/json", jsonModel, ParameterType.RequestBody);

                var restResponse = await client.ExecuteTaskAsync(request);
                if (restResponse.IsSuccessful)
                {                    
                    Loger.WriteMessage(LogPriority.Info, $"В запросе {client?.BaseUrl}: Данные отправлены");
                    return true;
                }
                else
                {
                    Loger.WriteMessage(LogPriority.Info, $"В запросе {client?.BaseUrl}: Данные не отправлены -> {restResponse.Content}");
                    return false;
                }

            }
            catch (Exception ex)
            {
                Loger.WriteMessage(LogPriority.Error, $"В запросе {client?.BaseUrl} Ошибка при отправке данных на сервер", ex.Message);
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
        public static IList<T> GetDataFromServer<T>(string nameMetod, object model = default(object))
        {
            var client = new RestClient(domain + $"/api/{nameMetod}");

            try
            {
                var request = new RestRequest(Method.POST);
                request.AddHeader("authorization", $"Bearer {AccessToken}");
                request.AddParameter("application/json", JsonConvert.SerializeObject(model), ParameterType.RequestBody);

                var restResponse = client.Execute(request);
                if (restResponse.IsSuccessful)
                {
                    Loger.WriteMessage(LogPriority.Info, $"{client?.BaseUrl} данные получены");
                    return JsonConvert.DeserializeObject<IList<T>>(restResponse.Content);
                }
                else
                {
                    Loger.WriteMessage(LogPriority.Info, $"{client?.BaseUrl} данные не получены: {restResponse.Content}");
                    return default(IList<T>);
                }
                
            }
            catch (Exception ex)
            {
                Loger.WriteMessage(LogPriority.Error, $"В запросе {client?.BaseUrl} Ошибка при получении данных с сервера", ex.Message);
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
            RestClient client = new RestClient(domain + $"/api/{nameMetod}");

            try
            {
                

                var request = new RestRequest(Method.POST);
                request.AddHeader("authorization", $"Bearer {AccessToken}");
                request.AddParameter("application/json", JsonConvert.SerializeObject(model), ParameterType.RequestBody);

                var restResponse = await client.ExecuteTaskAsync(request);

                if (restResponse.IsSuccessful)
                {                    
                    Loger.WriteMessage(LogPriority.Info, $"{client?.BaseUrl} данные получены");
                    return JsonConvert.DeserializeObject<IList<T>>(restResponse.Content);
                }
                else
                {
                    Loger.WriteMessage(LogPriority.Info, $"{client?.BaseUrl} данные не получены: {restResponse.Content}");
                    return default(IList<T>);
                }

            }
            catch (Exception ex)
            {
                Loger.WriteMessage(LogPriority.Error, $"В запросе {client?.BaseUrl} Ошибка при получении данных с червера", ex.Message);
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
                var client = new RestClient(domain + "/Token");

                var request = new RestRequest(Method.POST);
                request.AddParameter("grant_type", "password");
                request.AddParameter("username", login);
                request.AddParameter("password", password);
                var restResponse = client.Execute(request);
                if (restResponse.IsSuccessful)
                {
                    AccessToken = JsonConvert.DeserializeObject<AccessToken>(restResponse.Content).Access_token;                    
                }

                Loger.WriteMessage(LogPriority.Info, $"Статус авторизации: {restResponse.StatusCode}");
                return restResponse.StatusCode;
            }
            catch (Exception ex)
            {
                Loger.WriteMessage(LogPriority.Error, "Ошибка при получении токена", ex.Message);
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

        
        /// <summary>
        /// проверяет статус сервера
        /// </summary>
        /// <param name="nameMetodApi"></param>
        /// <returns></returns>
        public static HttpStatusCode CheckServerStatus(string nameMetodApi)
        {
            try
            {
                var client = new RestClient(domain + $"/api/ServerStatus/{nameMetodApi}");

                var request = new RestRequest(Method.GET);
                request.AddHeader("authorization", $"Bearer {AccessToken}");

                var restResponse = client.Execute(request).StatusCode;
                Loger.WriteMessage(LogPriority.Info, $"Запрос IsOnline -> {restResponse}");
                return restResponse;
                
            }
            catch (Exception ex)
            {
                Loger.WriteMessage(LogPriority.Error, "Ошибка при проверке статуса сервера", ex.Message);
                return default(HttpStatusCode);
            }
        }
    }

    public class AccessToken
    {
        public string Access_token { get; set; }
    }
}
