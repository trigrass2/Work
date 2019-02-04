using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using RestSharp;
using ToolsScanner.Model;

namespace ToolsScanner
{
    public static class RestApi
    {
        private static readonly string connectionString = "https://apiinfo.pik-industry.ru/api/";
        private static WebRequest wrAutorize;
        private static AccessToken token = new AccessToken();           

        public static List<Person> GetPersonList()
        {
            var client = new RestClient(connectionString + "Tools/GetPersonList");
            var request = new RestRequest(Method.GET);

            request.AddHeader("authorization", "Bearer " + GetToken());

            IRestResponse response = client.Execute(request);
            return JsonConvert.DeserializeObject<List<Person>>(response.Content);
        }

        public static List<Tool> GetToolsList()
        {
            var client = new RestClient(connectionString + string.Format("Tools/GetToolList"));
            var request = new RestRequest(Method.GET);

            request.AddHeader("authorization", "Bearer " + GetToken());

            IRestResponse response = client.Execute(request);
            return JsonConvert.DeserializeObject<List<Tool>>(response.Content);
        }

        public static string ChangeHolder(string person_id, string tool_id)
        {
            var client = new RestClient(connectionString + string.Format("Tools/ChangeHolder?person_id={0}&tool_id={1}", Convert.ToInt32(person_id), Convert.ToInt32(tool_id)));
            var request = new RestRequest(Method.GET);

            request.AddHeader("authorization", "Bearer " + GetToken());

            IRestResponse response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return "Отправлено!";
            }
            else
            {
                return "Не отправлено";
            }
        }

        private static string GetToken()
        {
            string reqString = "grant_type=password&username=agent@pik.ru&password=P@ss12";
            byte[] requestData = Encoding.UTF8.GetBytes(reqString);

            wrAutorize = WebRequest.Create("https://apiinfo.pik-industry.ru/token");
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