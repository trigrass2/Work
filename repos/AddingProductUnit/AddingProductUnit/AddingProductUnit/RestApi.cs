using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using RestSharp;

namespace AddingProductUnit
{
    public static class RestApi
    {
        private static readonly string connectionString = "https://pik-industry.info:81/api/";
        private static WebRequest wrAutorize;
        private static AccessToken token = new AccessToken();

        public static string NewProductEvent(string uniqueId, int regionId)
        {
            var client = new RestClient(connectionString + "ProductUnit/NewProductEvent?unique_id="+uniqueId+"&region_id="+regionId);
            var request = new RestRequest(Method.GET);
            request.RequestFormat = DataFormat.Json;

            request.AddHeader("authorization", "Bearer " + GetToken());

            IRestResponse response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return "Отправлено!";
            }
            else return "Не отправлено";
            
        }
        public static List<ProductRegion> GetProductRegionList()
        {
            var client = new RestClient(connectionString + "ProductUnit/GetProductRegionList");
            var request = new RestRequest(Method.GET);

            request.AddHeader("authorization", "Bearer " + GetToken());
            
            IRestResponse response = client.Execute(request);
            return JsonConvert.DeserializeObject<List<ProductRegion>>(response.Content);
        }

        public static void GetProductUniqueList(int unit_id)
        {
            var client = new RestClient(connectionString + "ProductUnit/GetProductUniqueList?unit_id=" + unit_id);
            var request = new RestRequest(Method.GET);            

            request.AddHeader("authorization", "Bearer " + GetToken());

            IRestResponse response = client.Execute(request);
        }

        public static ObservableCollection<ProductUnit> GetProductUnitList()
        {
            var client = new RestClient(connectionString + "ProductUnit/GetProductUnitList");
            var request = new RestRequest(Method.GET);

            request.AddHeader("authorization", "Bearer " + GetToken());

            IRestResponse response = client.Execute(request);
            return JsonConvert.DeserializeObject<ObservableCollection<ProductUnit>>(response.Content);
        }

        private static string GetToken()
        {
            string reqString = "grant_type=password&username=demo@demo.demo&password=Demo_123";
            byte[] requestData = Encoding.UTF8.GetBytes(reqString);

            wrAutorize = WebRequest.Create("https://pik-industry.info:81/token");
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