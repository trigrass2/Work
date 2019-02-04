using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using RestSharp;
using PCLAppConfig;

namespace Reports
{
    public static class RestApi
    {
        private static readonly string connectionString = "https://pik-industry.info:81/";
        private static WebRequest wrAutorize;
        private static  AccessToken token = new AccessToken();        

        public static void GetData()
        {
            var client = new RestClient(connectionString + "ProductUnit/NewProductEvent?unique_id=");
            var request = new RestRequest(Method.GET);
        }

        private static string GetTokken()
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
