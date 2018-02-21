using Chuvy.Data.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;

public class MessagesController : ApiController
{
    private readonly UnitofWork _unitofwork;

    public MessagesController()
    {
        _unitofwork = new UnitofWork(new ChuvyDbContext());
    }
    [HttpGet]
    [Route("api/Messages/Test")]
    [AllowAnonymous]
    public string Test()
    {
        return "Success";
    }

    [HttpPost]
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
