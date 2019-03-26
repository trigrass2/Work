using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Android.Net;
using Newtonsoft.Json;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using RestSharp;
using ServiceDesk.Models;

namespace ServiceDesk.PikApi
{
    /// <summary>
    /// Класс для работы с API
    /// </summary>
    public class ServiceDeskApi
    {
        private static readonly string connectionString = "https://apiinfo.pik-industry.ru/api/";

        public static AccessToken Token = new AccessToken();
        
        public static string AccessToken { get; private set; }
        

        /// <summary>
        /// Имя запроса
        /// </summary>
        public enum ApiEnum
        {
            GetTasks,
            GetTaskComments,
            CreateTask,
            AddTaskComment,
            EditTask,
            EditTaskComment,
            GetTypes,
            GetTaskAttachmentsInfo,
            AddTaskAttachment,
            GetTaskAttachments,
            GetUserInfo,
            Register1CProxy,
            GetProductFactoryList,
            GetProductPlantList,
            GetProductUnitList,
            GetUsersList,
            GetStatuses
        }

        #region SERVICE DESK

        public static IEnumerable<UserModel> GetAllUsers<T>(T user, ApiEnum nameApi)
        {
            RestClient client = new RestClient($"{connectionString}ServiceDesk/{Enum.GetName(typeof(ApiEnum), nameApi)}");
            RestRequest request = new RestRequest(Method.POST);

            request.AddHeader("authorization", $"Bearer {AccessToken}");
            request.RequestFormat = DataFormat.Json;

            string jsonUser = JsonConvert.SerializeObject(user);
            request.AddParameter("application/json", jsonUser, ParameterType.RequestBody);

            IRestResponse restResponse = client.Execute(request);

            if (restResponse.IsSuccessful)
            {
                return JsonConvert.DeserializeObject<IEnumerable<UserModel>>(restResponse.Content);
            }
            else return default(IEnumerable<UserModel>);
        }
        public static async Task<IEnumerable<UserModel>> GetAllUsersAsync<T>(T user, ApiEnum nameApi)
        {
            RestClient client = new RestClient($"{connectionString}ServiceDesk/{Enum.GetName(typeof(ApiEnum), nameApi)}");
            RestRequest request = new RestRequest(Method.POST);

            request.AddHeader("authorization", $"Bearer {AccessToken}");
            request.RequestFormat = DataFormat.Json;

            string jsonUser = JsonConvert.SerializeObject(user);
            request.AddParameter("application/json", jsonUser, ParameterType.RequestBody);

            IRestResponse restResponse = await client.ExecuteTaskAsync(request);

            if (restResponse.IsSuccessful)
            {
                return JsonConvert.DeserializeObject<IEnumerable<UserModel>>(restResponse.Content);
            }
            else return default(IEnumerable<UserModel>);
        }

        /// <summary>
        /// Отправляет данные на сервер
        /// </summary>
        /// <typeparam name="T">Тип отправляемых данных</typeparam>
        /// <param name="commentModel">Отправляемые данные</param>
        /// <param name="nameApi">Имя функции из API</param>
        public static void SendDataToServer<T>(T commentModel, ApiEnum nameApi)
        {
            RestClient client = new RestClient($"{connectionString}ServiceDesk/{Enum.GetName(typeof(ApiEnum), nameApi)}");
            RestRequest request = new RestRequest(Method.POST);

            request.AddHeader("authorization", $"Bearer {AccessToken}");
            request.RequestFormat = DataFormat.Json;

            string commentJson = JsonConvert.SerializeObject(commentModel);
            request.AddParameter("application/json", commentJson, ParameterType.RequestBody);

            IRestResponse restResponse = client.Execute(request);
        }

        /// <summary>
        /// Отправляет данные на сервер
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commentModel"></param>
        /// <param name="nameApi"></param>
        /// <returns></returns>
        public static async Task SendDataToServerAsync<T>(T commentModel, ApiEnum nameApi)
        {
            RestClient client = new RestClient($"{connectionString}ServiceDesk/{Enum.GetName(typeof(ApiEnum), nameApi)}");
            RestRequest request = new RestRequest(Method.POST);

            request.AddHeader("authorization", $"Bearer {AccessToken}");
            request.RequestFormat = DataFormat.Json;

            string commentJson = JsonConvert.SerializeObject(commentModel);
            request.AddParameter("application/json", commentJson, ParameterType.RequestBody);

            IRestResponse restResponse = await client.ExecuteTaskAsync(request);
        }

        /// <summary>
        /// Получает вложения
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="nameApi"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetTaskAttachments<T>(object model, ApiEnum nameApi)
        {
            RestClient client = new RestClient($"{connectionString}ServiceDesk/{Enum.GetName(typeof(ApiEnum), nameApi)}");
            RestRequest request = new RestRequest(Method.POST);

            request.AddHeader("authorization", $"Bearer {AccessToken}");
            request.RequestFormat = DataFormat.Json;

            string commentJson = JsonConvert.SerializeObject(model);
            request.AddParameter("application/json", commentJson, ParameterType.RequestBody);

            IRestResponse restResponse = client.Execute(request);
            if (restResponse.IsSuccessful)
            {
                return JsonConvert.DeserializeObject<IEnumerable<T>>(restResponse.Content);
            }
            else return default(IEnumerable<T>);
        }

        /// <summary>
        /// Возвращает данные по запросу
        /// </summary>
        /// <typeparam name="T">Тип возвращаемых данных</typeparam>
        /// <param name="nameApi">Название вызываемой функции</param>
        /// <returns></returns> 
        public static async Task<IEnumerable<T>> GetDataAsync<T>(ApiEnum nameApi)
        {
            RestClient client = new RestClient($"{connectionString}ServiceDesk/{Enum.GetName(typeof(ApiEnum), nameApi)}");
            RestRequest request = new RestRequest(Method.POST);

            request.AddHeader("authorization", $"Bearer {AccessToken}");
            request.RequestFormat = DataFormat.Json;

            IRestResponse restResponse = await client.ExecuteTaskAsync(request);

            if (restResponse.IsSuccessful)
            {
                return JsonConvert.DeserializeObject<IEnumerable<T>>(restResponse.Content);
            }
            else return default(IEnumerable<T>);
        }

        /// <summary>
        /// Возвращает данные о заявке
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="nameApi"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetData<T>(ApiEnum nameApi)
        {
            RestClient client = new RestClient($"{connectionString}ServiceDesk/{Enum.GetName(typeof(ApiEnum), nameApi)}");
            RestRequest request = new RestRequest(Method.POST);

            request.AddHeader("authorization", $"Bearer {AccessToken}");
            request.RequestFormat = DataFormat.Json;

            IRestResponse restResponse = client.Execute(request);

            if (restResponse.IsSuccessful)
            {
                return JsonConvert.DeserializeObject<IEnumerable<T>>(restResponse.Content);
            }
            else return default(IEnumerable<T>);
        }

        /// <summary>
        /// Возвращает данные по запросу используя ID заявки
        /// </summary>
        /// <typeparam name="T">Тип возвращаемых данных</typeparam>
        /// <param name="nameApi">Название вызываемой функции</param>
        /// <param name="TaskId">ID заявки</param>
        /// <returns></returns>
        public static async Task<IEnumerable<T>> GetDataFromTask<T>(ApiEnum nameApi, int TaskId)
        {
            RestClient client = new RestClient($"{connectionString}ServiceDesk/{Enum.GetName(typeof(ApiEnum), nameApi)}");
            RestRequest request = new RestRequest(Method.POST);

            request.AddHeader("authorization", $"Bearer {AccessToken}");
            request.RequestFormat = DataFormat.Json;

            string idTaskJson = "{\"Task_id\":" + TaskId + "}";
            request.AddParameter("application/json", idTaskJson, ParameterType.RequestBody);

            IRestResponse restResponse = await client.ExecuteTaskAsync(request);
            if (restResponse.IsSuccessful)
            {
                return JsonConvert.DeserializeObject<IEnumerable<T>>(restResponse.Content);
            }
            else return default(IEnumerable<T>);
        }
        #endregion

        #region SERVICE DESK MANAGMENT

        /// <summary>
        /// Асинхронно возвращает данные из системы управления ServiceDesk
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="nameApi"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<T>> GetDataServisDeskManagmentAsync<T>(ApiEnum nameApi)
        {
            RestClient client = new RestClient($"{connectionString}ServiceDeskManagement/{Enum.GetName(typeof(ApiEnum), nameApi)}");
            RestRequest request = new RestRequest(Method.POST);

            request.AddHeader("authorization", $"Bearer {AccessToken}");
            request.RequestFormat = DataFormat.Json;

            IRestResponse restResponse = await client.ExecuteTaskAsync(request);

            if (restResponse.IsSuccessful)
            {
                return JsonConvert.DeserializeObject<IEnumerable<T>>(restResponse.Content);
            }
            else return default(IEnumerable<T>);
        }

        /// <summary>
        /// Возвращает данные из системы управления ServiceDesk
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="nameApi"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetDataServisDeskManagment<T>(ApiEnum nameApi)
        {
            RestClient client = new RestClient($"{connectionString}ServiceDeskManagement/{Enum.GetName(typeof(ApiEnum), nameApi)}");
            RestRequest request = new RestRequest(Method.POST);

            request.AddHeader("authorization", $"Bearer {AccessToken}");
            request.RequestFormat = DataFormat.Json;

            IRestResponse restResponse = client.Execute(request);

            if (restResponse.IsSuccessful)
            {
                return JsonConvert.DeserializeObject<IEnumerable<T>>(restResponse.Content);
            }
            else return default(IEnumerable<T>);
        }

        #endregion


        #region PRODUCT UNIT

        /// <summary>
        /// Возвращает данные о производственных единицах
        /// </summary>
        /// <typeparam name="T">Модель возвращаемых данных</typeparam>
        /// <param name="nameApi">Имя функции API</param>
        /// <returns>Массив производственных единиц</returns>
        public static async Task<IEnumerable<T>> GetProductUnitAsync<T>(ApiEnum nameApi)
        {
            RestClient client = new RestClient($"{connectionString}ProductUnit/{Enum.GetName(typeof(ApiEnum), nameApi)}");
            RestRequest request = new RestRequest(Method.GET);

            request.AddHeader("authorization", $"Bearer {AccessToken}");
            request.RequestFormat = DataFormat.Json;

            IRestResponse restResponse = await client.ExecuteTaskAsync(request);

            if (restResponse.IsSuccessful)
            {
                return JsonConvert.DeserializeObject<IEnumerable<T>>(restResponse.Content);
            }
            else return default(IEnumerable<T>);
        }
        /// <summary>
        /// Возвращает данные о производственных единицах
        /// </summary>
        /// <typeparam name="T">Модель возвращаемых данных</typeparam>
        /// <param name="nameApi">Имя функции API</param>
        /// <returns>Массив производственных единиц</returns>
        public static IEnumerable<T> GetProductUnit<T>(ApiEnum nameApi)
        {
            RestClient client = new RestClient($"{connectionString}ProductUnit/{Enum.GetName(typeof(ApiEnum), nameApi)}");
            RestRequest request = new RestRequest(Method.GET);

            request.AddHeader("authorization", $"Bearer {AccessToken}");
            request.RequestFormat = DataFormat.Json;

            IRestResponse restResponse = client.Execute(request);

            if (restResponse.IsSuccessful)
            {
                return JsonConvert.DeserializeObject<IEnumerable<T>>(restResponse.Content);
            }
            else return default(IEnumerable<T>);
        }

        /// <summary>
        /// Возвращает данные о производственных единицах для отдельного завода
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="nameApi"></param>
        /// <param name="idFactory">id завода</param>
        /// <returns></returns>
        public static IEnumerable<T> GetProductUnit<T>(ApiEnum nameApi, int idFactory)
        {
            RestClient client = new RestClient($"{connectionString}ProductUnit/{Enum.GetName(typeof(ApiEnum), nameApi)}?factory_id={idFactory}");
            RestRequest request = new RestRequest(Method.GET);

            request.AddHeader("authorization", $"Bearer {AccessToken}");
            request.RequestFormat = DataFormat.Json;

            IRestResponse restResponse = client.Execute(request);

            if (restResponse.IsSuccessful)
            {
                return JsonConvert.DeserializeObject<IEnumerable<T>>(restResponse.Content);
            }
            else return default(IEnumerable<T>);
        }

        /// <summary>
        /// Возвращает данные о производственных единицах для отдельного завода и линии
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="nameApi"></param>
        /// <param name="idFactory"></param>
        /// <param name="idPlant"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetProductUnit<T>(ApiEnum nameApi, int idFactory, int idPlant)
        {
            RestClient client = new RestClient($"{connectionString}ProductUnit/{Enum.GetName(typeof(ApiEnum), nameApi)}?plant_id={idPlant}&factory_id={idFactory}");
            RestRequest request = new RestRequest(Method.GET);

            request.AddHeader("authorization", $"Bearer {AccessToken}");
            request.RequestFormat = DataFormat.Json;

            IRestResponse restResponse = client.Execute(request);

            if (restResponse.IsSuccessful)
            {
                return JsonConvert.DeserializeObject<IEnumerable<T>>(restResponse.Content);
            }
            else return default(IEnumerable<T>);
        }

        #endregion


        /// <summary>
        /// 1C авторизация
        /// </summary>
        /// <param name="nameApi"></param>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static HttpStatusCode Register1CProxy(ApiEnum nameApi, string login, string password)
        {
            RestClient client = new RestClient($"{connectionString}Auth/{Enum.GetName(typeof(ApiEnum), nameApi)}");
            RestRequest request = new RestRequest(Method.POST);

            request.RequestFormat = DataFormat.Json;

            string jsonUser = JsonConvert.SerializeObject(new { Login = login, Password = password });
            request.AddParameter("application/json", jsonUser, ParameterType.RequestBody);

            IRestResponse restResponse = client.Execute(request);

            if (restResponse.IsSuccessful)
            {
                Token = JsonConvert.DeserializeObject<AccessToken>(restResponse.Content);
                AccessToken = Token.access_token;
            }
            return restResponse.StatusCode;
        }

        public static HttpStatusCode LocalRegister(string login, string password)
        {
            WebRequest wrAutorize;
            string reqString = $"grant_type=password&username={login}&password={password}";
            byte[] requestData = Encoding.UTF8.GetBytes(reqString);

            wrAutorize = WebRequest.Create("https://apiinfo.pik-industry.ru/Token");
            wrAutorize.Method = "POST";
            wrAutorize.ContentType = "application/x-www-form-urlencoded";
            wrAutorize.ContentLength = requestData.Length;

            using (Stream S = wrAutorize.GetRequestStream())
                S.Write(requestData, 0, requestData.Length);

            var response = (HttpWebResponse)wrAutorize.GetResponse();
            using (response)
            {
                Token = JsonConvert.DeserializeObject<AccessToken>(new StreamReader(response.GetResponseStream()).ReadToEnd());
            }
            
            AccessToken = Token.access_token;

            return response.StatusCode;
        }

        #region GOOGLE API

        public static void RegisterExternalApiInfoPIK(string token)
        {            
            var client = new RestClient(connectionString + string.Format("Auth/RegisterExternal")); 
            var request = new RestRequest(Method.POST);
            
            request.RequestFormat = DataFormat.Json;

            var tokenJson = JsonConvert.SerializeObject(new { Provider = "Google", ExternalAccessToken = token });

            request.AddParameter("application/json", tokenJson, ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);
        }

        public static bool Error = false;
        public static HttpStatusCode LoginExternalService(string externaltoken)
        {          
            
            var client = new RestClient($"{connectionString}Auth/RegisterExternal")
            {
                Timeout = 5000
            };
            var request = new RestRequest(Method.POST);
            var tokenJson = JsonConvert.SerializeObject(new { Provider = "Google", ExternalAccessToken = externaltoken });
            request.AddParameter("application/json", tokenJson, ParameterType.RequestBody);

            IRestResponse restResponse = client.Execute(request);
            if (restResponse.IsSuccessful)
            {
                Token = JsonConvert.DeserializeObject<AccessToken>(restResponse.Content);
                AccessToken = Token.access_token;
            }
            return restResponse.StatusCode;
        }

        #endregion

        public static T GetUser<T>(ApiEnum nameApi)
        {
            RestClient client = new RestClient($"{connectionString}Auth/{Enum.GetName(typeof(ApiEnum), nameApi)}");
            RestRequest request = new RestRequest(Method.GET);

            request.AddHeader("authorization", $"Bearer {AccessToken}");
            request.RequestFormat = DataFormat.Json;

            IRestResponse restResponse = client.Execute(request);
            if (restResponse.IsSuccessful)
            {
                return JsonConvert.DeserializeObject<T>(restResponse.Content);
            }
            else return default(T);
        }        

    }

    public class AccessToken
    {
        public string access_token { get; set; }
    }
}
