using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
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
                string val = responseTask.Result.ToString();
            }
        }

        private static string GetUrl()
        {
            //return "http://chatbookabot.azurewebsites.net";
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

                var response = client.GetAsync("https://api.api.ai/v1/entities?v=20150910");
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
        private static void TestUpcomingAppointments()
        {
            var _unitofwork = new UnitofWork(new ChuvyDbContext());

            var appointments = _unitofwork.Appointments.GetAllUpcomingAppointments(30);


            Console.WriteLine(appointments.ToString());
            

        }
        private static void ProcessRequestWebFn()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:6492/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //var responseTask = client.SendAsync(request).Result;
                var responseTask = client.GetAsync("api/services/UploadServiceEntities");
                string val = responseTask.Result.ToString();
            }
        }
    }
    
}
