using System;
using System.Collections.Generic;
using System.IO;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ApiAiSDK;
using ApiAiSDK.Model;
using Chuvy.BotService.Test.apiai;
using NUnit.Framework.Api;
using NUnit.Framework.Internal.Execution;
using System.Web.Script.Serialization;

namespace Chuvy.BotService.Test
{
    
    class Program
    {
        
        static readonly HttpClient Webclient = new HttpClient();
        static  AIConfiguration config;

        static void Main(string[] args)
        {
            ProcessRequest();
            //TestEntities();
        }
        private static void ProcessRequest()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(GetUrl());
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "botservice");
                var httpContent = new StringContent(InputRequest(), Encoding.UTF8, "application/json");

                //var responseTask = client.SendAsync(request).Result;
                var responseTask = client.PostAsync("botservice", httpContent);
                string val = responseTask.Result.ToString();
            }
        }

        private static string GetUrl()
        {
            //return "http://chatbookabot.azurewebsites.net";
            return "http://localhost:12995/";
        }

        private static string InputRequest()
        {
            using (var r = new StreamReader("../../json/fullfillment.json"))
            {
                return r.ReadToEnd();
            }
        }
        private static void TestEntities()
        {
            using (HttpClient client = new HttpClient())
            {                
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + "9b66dae2ac8644bbb9c24b7c19d92234");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = client.GetAsync("https://api.api.ai/v1/entities?v=20150910");
                var res = response.Result;
                var s = res.Content.ReadAsStringAsync();
            }
        }

    }
}
