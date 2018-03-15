using Chuvy.Data.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;

namespace DialogFlowTest.Controllers
{
    public class MessagesController : ApiController
    {
        private readonly UnitofWork _unitofwork;

        public MessagesController()
        {
            _unitofwork = new UnitofWork(new ChuvyDbContext());
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
                string senderId = value.ToString();
                var latestMessage = _unitofwork.Messages.Find(x => x.SenderId == senderId).OrderByDescending(x => x.CreateDateTime).FirstOrDefault();

                if (latestMessage != null && (latestMessage.ActionName.Contains("Checking") || latestMessage.ActionName.Contains("CheckingConfirmation")))
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

        // POST: api/Messages
        public string Post([FromUri]string value)
        {
            return "";
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
    }
}
