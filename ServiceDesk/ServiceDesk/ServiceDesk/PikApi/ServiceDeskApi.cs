using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using ServiceDesk.Models;

namespace ServiceDesk.PikApi
{
    /// <summary>
    /// Класс для работы с API
    /// </summary>
    public static class ServiceDeskApi
    {
        private static readonly string connectionString = "http://10.5.0.225/api/";       
        private static AccessToken token = new AccessToken();

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
            GetTaskAttachments
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

            request.AddHeader("authorization", $"Bearer {GetToken()}");
            request.RequestFormat = DataFormat.Json;

            string commentJson = JsonConvert.SerializeObject(commentModel);
            request.AddParameter("application/json", commentJson, ParameterType.RequestBody);

            IRestResponse restResponse = client.Execute(request);            
        }

        public static IEnumerable<T> GetTaskAttachments<T>(object commentModel, ApiEnum nameApi)
        {
            RestClient client = new RestClient($"{connectionString}ServiceDesk/{Enum.GetName(typeof(ApiEnum), nameApi)}");
            RestRequest request = new RestRequest(Method.POST);

            request.AddHeader("authorization", $"Bearer {GetToken()}");
            request.RequestFormat = DataFormat.Json;

            string commentJson = JsonConvert.SerializeObject(commentModel);
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
        public static async Task<IEnumerable<T>>  GetDataFromTask<T>(ApiEnum nameApi)
        {
            RestClient client = new RestClient($"{connectionString}ServiceDesk/{Enum.GetName(typeof(ApiEnum), nameApi)}");
            RestRequest request = new RestRequest(Method.POST);

            request.AddHeader("authorization", $"Bearer {GetToken()}");
            request.RequestFormat = DataFormat.Json;
                        
            IRestResponse restResponse = await client.ExecuteTaskAsync(request);
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

            request.AddHeader("authorization", $"Bearer {GetToken()}");
            request.RequestFormat = DataFormat.Json;

            string idTaskJson = "{\"Task_id\":"+TaskId+"}";
            request.AddParameter("application/json", idTaskJson, ParameterType.RequestBody);

            IRestResponse restResponse = await client.ExecuteTaskAsync(request);
            if (restResponse.IsSuccessful)
            {               
                return JsonConvert.DeserializeObject<IEnumerable<T>>(restResponse.Content);
            }
            else return default(IEnumerable<T>);
        }

        /// <summary>
        /// Get access token
        /// </summary>
        /// <returns></returns>
        private static string GetToken()
        {
            string reqString = "grant_type=password&username=test@pik.ru&password=P@ss12";
            byte[] requestData = Encoding.UTF8.GetBytes(reqString);

            var wrAutorize = WebRequest.Create("http://10.5.0.225/token");
            wrAutorize.Method = "POST";
            wrAutorize.ContentType = "application/x-www-form-urlencoded";
            wrAutorize.ContentLength = requestData.Length;

            using (Stream S = wrAutorize.GetRequestStream())
                S.Write(requestData, 0, requestData.Length);

            using (var response = (HttpWebResponse)wrAutorize.GetResponse())
            {
                token.access_token = new StreamReader(response.GetResponseStream()).ReadToEnd();
            }

            token = JsonConvert.DeserializeObject<AccessToken>(token.access_token);

            return token.access_token;

        }
    }

    public class AccessToken
    {
        public string access_token { get; set; }
    }
}
