using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ApiAiSDK;
using Chuvy.Data.Persistence;

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
            //ProcessRequestWeb();
            //TestUpcomingAppointments();
            //ProcessRequestWebFn();
            //UploadServiceEntities();
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
                var httpContent = new StringContent(InputRequest("../../json/fullfillment.json"), Encoding.UTF8, "application/json");

                //var responseTask = client.SendAsync(request).Result;
                var responseTask = client.PostAsync("botservice", httpContent);
                string val = responseTask.Result.Content.ReadAsStringAsync().Result.ToString();
                Console.WriteLine(val);
                Console.ReadLine();
            }
        }

        private static string GetUrl()
        {
            //return "http://bookastagingagent.azurewebsites.net/";
            return "http://localhost:12995/";
        }

        private static string InputRequest(string fileName)
        {
            using (var r = new StreamReader(fileName))
            {
                return r.ReadToEnd();
            }
        }
        private static void TestEntities()
        {
            using (HttpClient client = new HttpClient())
            {                
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + "9cd21fc46a5a4a7c8a68fe78ff4cc4de");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = client.GetAsync("https://api.dialogflow.com/v1/entities?v=20150910");
                var res = response.Result;
                var s = res.Content.ReadAsStringAsync();
            }
        }

        private static void ProcessRequestWeb()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:6492/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "api/appointments/Channel");
                var httpContent = new StringContent(InputRequest("../../json/changesCronofy.json"), Encoding.UTF8, "application/json");

                //var responseTask = client.SendAsync(request).Result;
                var responseTask = client.PostAsync("api/appointments/Channel", httpContent);
                string val = responseTask.Result.ToString();
            }
        }
        //private static void TestUpcomingAppointments()
        //{
        //    var _unitofwork = new UnitofWork(new ChuvyDbContext());

        //    var appointments = _unitofwork.Appointments.GetAllUpcomingAppointments(30);


        //    Console.WriteLine(appointments.ToString());
            

        //}
        private static void ProcessRequestWebFn()
        {
            using (HttpClient client = new HttpClient())
            {
                //client.BaseAddress = new Uri("http://bookaprodonboarding.azurewebsites.net/");
                client.BaseAddress = new Uri("http://localhost:6492/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //var responseTask = client.SendAsync(request).Result;
                var responseTask = client.GetAsync("api/services/UploadServiceEntities");
                string val = responseTask.Result.Content.ReadAsStringAsync().Result;
            }
        }

        public static void UploadServiceEntities()
        {
            var apiAiClientAccessToken = "9cd21fc46a5a4a7c8a68fe78ff4cc4de";
            var apiAiClientBaseUri = "https://api.dialogflow.com/v1/entities/c8bf3b30-41e5-4e14-97d3-784a6be94f44/entries?v=20150910";


            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + apiAiClientAccessToken);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var httpContent = new StringContent(InputRequest("../../json/EntitiesJson.json"), Encoding.UTF8, "application/json");

                var response = client.PutAsync(apiAiClientBaseUri, httpContent);
                var result = response.Result.Content.ReadAsStringAsync().Result;
            }
        }

        static async Task<string> RunMethods()
        {
            var apiAiClientAccessToken = "9cd21fc46a5a4a7c8a68fe78ff4cc4de";
            var apiAiClientBaseUri = "https://api.dialogflow.com/v1/entities/c8bf3b30-41e5-4e14-97d3-784a6be94f44/entries?v=20150910";

            var data = @"[  {    \""synonyms\"": [      \""uat testing\"", \""uattesting\""    ],    \""value\"": \""UAT testing\""  }]";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + apiAiClientAccessToken);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var httpContent = new StringContent(InputRequest("../../json/EntitiesJson.json"), Encoding.UTF8, "application/json");

                var response = client.PutAsync(apiAiClientBaseUri, httpContent);
                var result = response.Result.Content.ReadAsStringAsync().Result;
                return result;
            }
        }
    }    
}
