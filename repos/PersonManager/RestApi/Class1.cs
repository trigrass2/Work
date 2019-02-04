using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using RestSharp;

namespace RestApi
{
    public static class Class1
    {
        private static readonly string connectionString = "https://apiinfo.pik-industry.ru/api/";
        private static WebRequest wrAutorize;
        private static AccessToken token = new AccessToken();

        public static void AddOrUpdatePerson(C1CPersonBindingModel person)
        {
            var client = new RestClient(connectionString + string.Format("Agent1C/AddOrUpdatePerson"));
            var request = new RestRequest(Method.POST);            

            request.AddHeader("authorization", "Bearer " + GetToken());
            request.RequestFormat = DataFormat.Json;

            var personJson = request.JsonSerializer.Serialize(person);

            request.AddParameter("application/json", personJson, ParameterType.RequestBody);
            
            IRestResponse response = client.Execute(request);            

        }

        public static void AddOrUpdatePersonStatus(C1CPersonGroupBindingModel c1CPersonGroupBindingModel)
        {
            var client = new RestClient(connectionString + string.Format("Agent1C/AddOrUpdatePersonStatus"));
            var request = new RestRequest(Method.POST);

            request.AddHeader("authorization", "Bearer " + GetToken());
            request.RequestFormat = DataFormat.Json;

            var c1c = request.JsonSerializer.Serialize(c1CPersonGroupBindingModel);

            request.AddParameter("application/json", c1c, ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);

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
