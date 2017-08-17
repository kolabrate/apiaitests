using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apiai.tests
{

    public class Rootobject
    {
        public Originalrequest originalRequest { get; set; }
        public string id { get; set; }
        public DateTime timestamp { get; set; }
        public string lang { get; set; }
        public Result result { get; set; }
        public Status status { get; set; }
        public string sessionId { get; set; }
    }

    public class Originalrequest
    {
        public string source { get; set; }
        public Data data { get; set; }
    }

    public class Data
    {
        public string ApiVersion { get; set; }
        public string SmsSid { get; set; }
        public string SmsStatus { get; set; }
        public string SmsMessageSid { get; set; }
        public string NumSegments { get; set; }
        public string ToState { get; set; }
        public string From { get; set; }
        public string MessageSid { get; set; }
        public string AccountSid { get; set; }
        public string ToCity { get; set; }
        public string FromCountry { get; set; }
        public string ToZip { get; set; }
        public string FromCity { get; set; }
        public string To { get; set; }
        public string FromZip { get; set; }
        public string ToCountry { get; set; }
        public string Body { get; set; }
        public string NumMedia { get; set; }
        public string FromState { get; set; }
    }

    public class Result
    {
        public string source { get; set; }
        public string resolvedQuery { get; set; }
        public string speech { get; set; }
        public string action { get; set; }
        public bool actionIncomplete { get; set; }
        public Parameters parameters { get; set; }
        public Context[] contexts { get; set; }
        public Metadata metadata { get; set; }
        public Fulfillment fulfillment { get; set; }
        public float score { get; set; }
    }

    public class Parameters
    {
        public string date { get; set; }
        public string time { get; set; }
    }

    public class Metadata
    {
        public string intentId { get; set; }
        public string webhookUsed { get; set; }
        public string webhookForSlotFillingUsed { get; set; }
        public string intentName { get; set; }
    }

    public class Fulfillment
    {
        public string speech { get; set; }
        public Message[] messages { get; set; }
    }

    public class Message
    {
        public int type { get; set; }
        public string speech { get; set; }
    }

    public class Context
    {
        public string name { get; set; }
        public Parameters1 parameters { get; set; }
        public int lifespan { get; set; }
    }

    public class Parameters1
    {
        public string date { get; set; }
        public string timeoriginal { get; set; }
        public string dateoriginal { get; set; }
        public string beforeAfteroriginal { get; set; }
        public string serviceoriginal { get; set; }
        public string beforeAfter { get; set; }
        public string Bookingtimeperiod { get; set; }
        public string Bookingdateperiodoriginal { get; set; }
        public string Bookingtimeperiodoriginal { get; set; }
        public string Bookingdateoriginal { get; set; }
        public string Bookingdate { get; set; }
        public string service { get; set; }
        public string time { get; set; }
        public string Bookingtime { get; set; }
        public string Bookingdateperiod { get; set; }
        public string Bookingtimeoriginal { get; set; }
    }

    public class Status
    {
        public int code { get; set; }
        public string errorType { get; set; }
    }

    class Class1
    {
    }
}
