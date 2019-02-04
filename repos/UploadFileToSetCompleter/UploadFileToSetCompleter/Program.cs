using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;

namespace UploadFileToSetCompleter
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "";
           
            AccessToken token = new AccessToken();

        }

        void UploadFiles(string fileName, AccessToken token)
        {
            var client = new RestClient("");
            var request = new RestRequest(Method.POST);

            request.AddHeader("authorization", "Bearer " + token);
            request.RequestFormat = DataFormat.Json;
        }

        string GetToken()
        {
            AccessToken token = new AccessToken();
            string reqString = "grant_type=password&username=??&password=??";
            byte[] requestData = Encoding.UTF8.GetBytes(reqString);

            WebRequest wrAutorize = WebRequest.Create("https://pik-industry.info:81/token");
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
