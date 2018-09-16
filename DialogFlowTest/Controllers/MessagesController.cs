using ApiAiSDK;
using ApiAiSDK.Model;
using Chuvy.Data.Persistence;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace DialogFlowTest.Controllers
{
    public class MessagesController : ApiController
    {
        private UnitofWork _unitofwork;
        private const string accessToken = "3e6c9aff32be480aabf56bfd796929e5";
        //private static string accessToken = "e0cdd76a123a48f194e626e15af15c02";
        private static ApiAi apiAi;
        private static AIDataService dataService;
        private static AIResponse response = null;
        public MessagesController()
        {
            var config = new AIConfiguration(accessToken, SupportedLanguage.English);
            dataService = new AIDataService(config);
        }
        // GET: api/Messages
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Messages/5
        public string Get(string value)
        {
            try
            {
                _unitofwork = new UnitofWork(new ChuvyDbContext());
                string senderId = value.ToString();
                var latestMessage = _unitofwork.Messages.Find(x => x.SenderId == senderId).OrderByDescending(x => x.CreateDateTime).FirstOrDefault();

                if (latestMessage != null && (latestMessage.ActionName.Contains("ServiceAvailabilityChecked")))
                {
                    var bookaReplies = latestMessage.BookaReply.Split(';');
                    if (bookaReplies.Length >= 1)
                    {
                        return bookaReplies[1];
                    }
                    else
                    {
                        Thread.Sleep(5000);
                        latestMessage = _unitofwork.Messages.Find(x => x.SenderId == senderId).OrderByDescending(x => x.CreateDateTime).FirstOrDefault();

                        bookaReplies = latestMessage.BookaReply.Split(';');
                        if (bookaReplies.Length >= 1)
                        {
                            return bookaReplies[1];
                        }
                        else
                        {
                            return "";
                        }
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                return "Error in reading second message" + ex.ToString();
            }
        }

        [HttpGet]
        public async Task<AIResponse> SendTextAsync(string value,string number)
        {
            var request = new AIRequest(value);
            request.OriginalRequest = PopulateOriginalRequest(number);
            response = await dataService.RequestAsync(request);

            if (response.Status.Code == 200)
            {

                return response;
            }
            else
            {
                return response;
            }
        }        

        // PUT: api/Messages/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Messages/5
        public void Delete(int id)
        {
        }

        [HttpGet]
        [Route("api/Messages/SendMessage")]
        [AllowAnonymous]
        public string CheckAdditionalMessages(string fromNum)
        {
            var replies = new List<string>();

            var latestMessage = _unitofwork.Messages.Find(x => x.SenderId == fromNum).OrderByDescending(x => x.CreateDateTime).FirstOrDefault();

            if (latestMessage.ActionName.Contains("Checking") || latestMessage.ActionName.Contains("CheckingConfirmation"))
            {
                var bookaReplies = latestMessage.BookaReply.Split(';');
                if (bookaReplies.Length >= 1)
                {
                    replies.Add(bookaReplies[1]);
                }
                else
                {
                    Thread.Sleep(5000);
                    latestMessage = _unitofwork.Messages.Find(x => x.SenderId == fromNum).OrderByDescending(x => x.CreateDateTime).FirstOrDefault();

                    bookaReplies = latestMessage.BookaReply.Split(';');
                    if (bookaReplies.Length >= 1)
                    {
                        replies.Add(bookaReplies[1]);
                    }
                }
            }
            return latestMessage.AiRawMessage;
        }
        private OriginalRequest PopulateOriginalRequest(string userQuery, string number = "+61400000000")
        {
            return new OriginalRequest()
            {
                Data = new
                {
                    ApiVersion = "",
                    SmsSid = "",
                    SmsStatus = "",
                    SmsMessageSid = "",
                    NumSegments = "",
                    ToState = "",
                    From = number, //a temp phone number that represents its from console app.
                    MessageSid = "SMf0000000000000000000000",//a rep of console.
                    AccountSid = "",
                    ToCity = "",
                    FromCountry = "",
                    ToZip = "",
                    FromCity = "",
                    To = "",
                    FromZip = "",
                    ToCountry = "",
                    Body = userQuery,
                    NumMedia = "",
                    FromState = ""
                },
                Source = "console"
            };

        }
    }
}
