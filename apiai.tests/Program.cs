using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ApiAiSDK;
using ApiAiSDK.Model;
using System.IO;
using Newtonsoft.Json;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace apiai.tests
{
    class Program
    {
        private const string accessToken = "9b66dae2ac8644bbb9c24b7c19d92234";
        //private static string accessToken = "e0cdd76a123a48f194e626e15af15c02";
        private static ApiAi apiAi;
        private static AIDataService dataService;
        static void Main(string[] args)
        {
            try
            {
                //IndividualConvo();
                AllConversations();
                //AllConversationsViaTwilio();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.Read();
                
            }

        }

        private static void AllConversationsViaTwilio()
        {
            // Find your Account Sid and Auth Token at twilio.com/console
            const string accountSid = "ACf6f7442e6e1a2d2d1072f60b7ad1b30b";
            const string authToken = "1e9868706fecd711c1746e38162e11e1";
            TwilioClient.Init(accountSid, authToken);
            var fromNum = "+15005550006";

            MessageResource.Create(
    from: new PhoneNumber(fromNum),
    to: new PhoneNumber("+61451562335"),
    body: "Hello");

            var messages = MessageResource.Read();
            var m = messages.ToList().Take(2);
            foreach (var message in m)
            {                
                Console.WriteLine(message.Direction+": "+ message.Body);
            }
            Console.ReadLine();
        }
        
        private static void IndividualConvo()
        {
            Initialise();
            do
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("user:");
                UserQuery = Console.ReadLine();
                if (!string.IsNullOrEmpty(UserQuery))
                {
                    ProcessUserQueryAsync().Wait();
                    Console.ForegroundColor = IsError == true
                        ? ConsoleColor.Red
                        : ConsoleColor.DarkGreen;
                    Console.Write($"Booka:{BookaResponse} \n");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.Write($"Booka:Please enter a message(or) enter 'end' to end the chat \n");
                }



            } while (UserQuery != "end");
        }


        private static async Task ProcessUserQueryAsync(string number = "+61400000000")
        {
            try
            {

                var response = GetResponse(UserQuery,number).Result;
                if (response.IsError == false)
                {
                    BookaResponse = response.Result.Fulfillment.Speech;
                    BookaResponseDisplayText = response.Result.Fulfillment.DisplayText;
                }
                else
                {
                    IsError = true;
                    BookaResponse = "Sorry something went wrong, please try again later.1";
                }
            }
            catch (Exception ez)
            {
                //resetContexts();
                IsError = true;
                BookaResponse = ez.Message+ez.InnerException.Message;
            }
           
            // BookaResponse = response.Result.Fulfillment.Speech;
            
        }
        private static AIResponse response = null;
        private static async Task<AIResponse> GetResponse(string userQuery,string number = "+61400000000")
        {
            var request = new AIRequest(userQuery);
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



        #region private methods to populate object

        private static void Initialise()
        {
            var config = new AIConfiguration(accessToken, SupportedLanguage.English);
            dataService = new AIDataService(config);
        }
        private static OriginalRequest PopulateOriginalRequest(string number = "+61400000000")
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
                    Body = UserQuery,
                    NumMedia = "",
                    FromState = ""
                },
                Source = "console"
            };

        }
        private static string UserQuery { get; set; }
        private static string BookaResponse { get; set; }
        private static string BookaResponseDisplayText { get; set; }
        private static bool IsError { get; set; }

        public static void AllConversations()
        {
            var greetingVariations = new List<string>();
            var appmtRequestVariations = new List<string>();
            var appmtRequestConfirmVariations = new List<string>();
            var variations = new Dictionary<string, List<string>>();
            var exits = Directory.Exists(@"..\Data\");
            var list = new List<string>();
            var fileNames = new List<string>();
            var filesNamesArray = Directory.GetFiles(@"..\..\Data\");
            fileNames = filesNamesArray.Where(c => c.StartsWith("..\\..\\Data\\TC")).ToList();

            //fileNames.Add(@"..\..\Data\TC1_ExistingCustomerForBookedSlots.txt");
            //fileNames.Add(@"..\..\Data\TC10_NewCustomerExceptions.txt");
            //fileNames.Add(@"..\..\Data\TC2_ExistingCustomerForFreeSlots.txt");
            //fileNames.Add(@"..\..\Data\TC3_NewCustomerForBookedSlots.txt");
            //fileNames.Add(@"..\..\Data\TC4_NewCustomerForFreeSlots.txt");
            //fileNames.Add(@"..\..\Data\TC41_NewCustomerForFreeSlots.txt");
            //fileNames.Add(@"..\..\Data\TC5_ConfirmationVariations.txt");
            //fileNames.Add(@"..\..\Data\TC6_ExistingCustomerShowMore.txt");
            //fileNames.Add(@"..\..\Data\TC7_NewCustomerForFreeSlots.txt");
            //fileNames.Add(@"..\..\Data\TC8_ConfirmationVariations.txt");
            //fileNames.Add(@"..\..\Data\TC81_ExistingCustomerShowMore.txt");
            //fileNames.Add(@"..\..\Data\TC9_ExistingCustomerShowMore.txt");

            foreach (var fileName in fileNames)
            {
                var allLines = File.ReadAllLines(fileName);
                list.AddRange(allLines);
            }
            var expressions = new List<Expression>();
            var newExpressions = new List<Expression>();
            Expression currentExpr = null;
            Case currentCase = null;
            Conversation currentConvo = null;

            #region Build Expressions
            foreach (var str in list)
            {
                if (!str.StartsWith("#") && !string.IsNullOrEmpty(str.Trim()))
                {
                    if (str.StartsWith("variations:"))
                    {
                        var firstPortion = str.IndexOf("variations: Name=") > -1 ? "variations: Name=" : "variations:Name=";
                        var lastPortion = str.IndexOf("; Values=") > -1 ? "; Values=" : ";Values=";
                        var variationName = str.Replace(firstPortion, "");
                        var endofName = variationName.IndexOf(lastPortion);
                        variationName = variationName.Substring(0, endofName);
                        var variationValuesStr = str.Replace(firstPortion + variationName + lastPortion, "");
                        var variationValues = variationValuesStr.Split(',');
                        var listValues = new List<string>();
                        foreach (var val in variationValues)
                        {
                            listValues.Add(val);
                        }
                        variations.Add(variationName, listValues);
                    }
                    //if (str.Contains("Name=Greeting;"))
                    //{
                    //    var val = str.Replace("variations: Name=Greeting; Values=", "");
                    //    var values = val.Split(',');
                    //    foreach (var greetingVal in values)
                    //    {
                    //        greetingVariations.Add(greetingVal);
                    //    }
                    //}
                    //if (str.Contains("Name=AppmtRequest;"))
                    //{
                    //    var val = str.Replace("variations: Name=AppmtRequest; Values=", "");
                    //    var values = val.Split(',');
                    //    foreach (var aVal in values)
                    //    {
                    //        appmtRequestVariations.Add(aVal);
                    //    }
                    //}
                    //if (str.Contains("Name=AppmtRequestConfirm;"))
                    //{
                    //    var val = str.Replace("variations: Name=AppmtRequestConfirm; Values=", "");
                    //    var values = val.Split(',');
                    //    foreach (var aval in values)
                    //    {
                    //        appmtRequestConfirmVariations.Add(aval);
                    //    }
                    //}
                    if (str.Contains("Expression:"))
                    {
                        var exprName = str.Replace("Expression:", "");
                        currentExpr = new Expression()
                        {
                            Name = exprName,
                            Cases = new List<Case>(),
                            IsNewCustomer = exprName.Contains("(New Customer)")
                        };
                        newExpressions.Add(currentExpr);
                    }
                    if (str.Contains("Case:"))
                    {
                        var caseName = str.Replace("Case:", "");
                        currentCase = new Case()
                        {
                            Name = caseName,
                            Conversations = new List<Conversation>(),
                            ExpressionName = currentExpr.Name
                        };
                        currentExpr.Cases.Add(currentCase);
                    }
                    if (str.StartsWith("Customer:") || str.StartsWith("Booka:"))
                    {
                        var text = str.Replace("Customer:", "");
                        text = text.Replace("Booka:", "");
                        currentConvo = new Conversation()
                        {
                            Text = text,
                            Type = str.Contains("Customer:") ? ConvoType.Request : ConvoType.Response
                        };
                        currentCase.Conversations.Add(currentConvo);
                    }
                }
            }
            #endregion

            #region All convos 
            var fs = File.Create(@"..\..\Data\results.txt");
            fs.Close();

            foreach (var expression in newExpressions)
            {
                foreach (var thisCase in expression.Cases)
                {
                    ProcessCase(thisCase.Conversations, thisCase.Name,expression.IsNewCustomer);
                }
            }
            
            #endregion

            #region New Process Expressions 
            var masterCaseList = new List<Case>();
            var allGreetingCaseFull = new List<Case>();
            foreach (var expression in expressions)
            {
                var thisCases = expression.Cases;
                foreach (var thisCase in thisCases)
                {                    
                    var processedCases = new List<Case>();
                    if (allGreetingCaseFull.Count > 0)
                    {
                        masterCaseList.AddRange(allGreetingCaseFull);
                    }
                    processedCases.Add(thisCase);
                    foreach (var variation in variations)
                    {
                        var key = variation.Key;
                        var variationList = variation.Value;
                        var allGreetingCases = new List<Case>();
                        foreach (var variationItem in variationList)
                        {
                            foreach (var processedCase in processedCases)
                            {
                                string tmp = JsonConvert.SerializeObject(processedCase);
                                var copyCase = JsonConvert.DeserializeObject<Case>(tmp);                                
                                var con = copyCase.Conversations.FirstOrDefault(c => c.Text.Contains(key));
                                if (con != null)
                                {
                                    con.Text = con.Text.Replace("[" + key + "]", variationItem);
                                    allGreetingCases.Add(copyCase);
                                }
                            }                            
                        }
                        if (allGreetingCases.Count > 0)
                        {
                            processedCases = allGreetingCases;
                            allGreetingCaseFull = new List<Case>();
                            allGreetingCaseFull.AddRange(processedCases);
                        }
                    }
                    //thisCases = allGreetingCaseFull;
                }
            }
            masterCaseList.AddRange(allGreetingCaseFull);
            var dateList = new List<string>();
            var currentDt = new DateTime(2017, 08, 30, 12, 30, 00);
            var lastDt = new DateTime(2017, 09, 02, 17, 00, 00);
            while (currentDt != lastDt)
            {

                if (currentDt.Hour >= 9 && currentDt.TimeOfDay <= new TimeSpan(17, 0, 0))
                {
                    
                    var dtString = currentDt.Day.ToString() + GetDaySuffix(currentDt.Day) + " " + currentDt.ToString("MMM hh:mm tt").Substring(0, 3) + " at " + currentDt.ToString("hh:mm tt");
                    for (int i = 0; i < 4; i++)
                    {
                        dateList.Add(dtString);
                    }
                }
                currentDt = currentDt.AddMinutes(10);
            }
            var freeSlotCases = masterCaseList.Where(c => c.Name.Contains("free time slot")).ToList();
            var bookedSlotCases = masterCaseList.Where(c => c.Name.Contains("booked time slot")).ToList();

            int ji = 0;
            foreach (var caseItem in freeSlotCases)
            {
                freeDt = dateList[ji];
                ji = ji + 1;
                var caseName = string.Format("Case: {0} (Expression: {1})", caseItem.Name,caseItem.ExpressionName);
                Console.WriteLine(caseName);
                WriteToFile(caseName);
                ProcessCase(caseItem.Conversations, caseItem.Name);
            }
            serviceName = "reports dev";
            foreach (var caseItem in bookedSlotCases)
            {
                var caseName = string.Format("Case: {0} (Expression: {1})", caseItem.Name, caseItem.ExpressionName);
                Console.WriteLine(caseName);
                WriteToFile(caseName);
                ProcessCase(caseItem.Conversations, caseItem.Name);
            }
            #endregion

            #region Process Expressions
            //greetingVariations = variations["Greeting"];
            //appmtRequestConfirmVariations = variations["AppmtRequestConfirm"];
            //appmtRequestVariations = variations["AppmtRequest"];
            //var fullDateTimeVariations = variations["FullDateTime"];

            //var dates = new List<DateTime>();
            //var currentDt = new DateTime(2017, 08, 24, 16, 00, 00);
            //var lastDt = new DateTime(2017, 08, 26, 17, 00, 00);
            //while (currentDt != lastDt)
            //{
            //    if (currentDt.Hour >= 18)
            //    { }
            //    else
            //    {
            //        dates.Add(currentDt);
            //    }
            //    currentDt = currentDt.AddMinutes(30);

            //}


            //foreach (var expression in expressions)
            //{
            //    WriteToFile(string.Format("Expression: {0}", expression.Name));
            //    foreach (var caseItem in expression.Cases)
            //    {
            //        WriteToFile("********************************************************************************************************************************************************");
            //        WriteToFile(string.Format("Case: {0}", caseItem.Name));
            //        //if (caseItem.Conversations.Exists(x => x.Text.Contains("[Greeting]")))
            //        //{     
            //        foreach (var aReqConfirm in appmtRequestConfirmVariations)
            //        {
            //            foreach (var greetingVariation in greetingVariations)
            //            {
            //                foreach (var aReq in appmtRequestVariations)
            //                {
            //                    //foreach (var dt in fullDateTimeVariations)
            //                    //{
            //                    foreach (var date in dates)
            //                    {
            //                        bookedDt = date.Day+ GetDaySuffix(date.Day)+ " "+date.ToString("MMM")+" at "+date.ToString("hh:mm tt");
            //                        freeDt = bookedDt;
            //                        ProcessCase(caseItem.Conversations, greetingVariation, aReq, aReqConfirm, caseItem.Name);
            //                    }
            //                    //}
            //                }
            //            }
            //        }
            //        //}
            //        //else
            //        //{
            //        //    ProcessCase(caseItem.Conversations, null, null, null, caseItem.Name);
            //        //}
            //        WriteToFile("********************************************************************************************************************************************************");
            //    }
            //}



            #endregion
            Console.WriteLine("DONE");
            Console.ReadLine();
        }
        private static string GetDaySuffix(int day)
        {
            if (day > 0)
            {
                if (day % 10 == 1 && day % 100 != 11)
                    return "st";
                else if (day % 10 == 2 && day % 100 != 12)
                    return "nd";
                else if (day % 10 == 3 && day % 100 != 13)
                    return "rd";
                else
                    return "th";
            }
            else
                return string.Empty;
        }
        private static string bookedDt = "22nd Sep at 12PM";
        private static string freeDt = "22nd Sep at 12PM";
        private static string freeDtConfirm = "3.30 pm on 21st Sep";
        private static string serviceName = "henna";
        private static string providerName = "Kolabrate";
        private static void ProcessCase(List<Conversation> conversations, string caseName, bool isNewCustomer = true)
        {
            response = null;
            Initialise();
            var number = "+61400000000";
            if (isNewCustomer)
                number = "+61" + (new string(DateTime.Now.Ticks.ToString().Reverse().ToArray())).Substring(0, 8);

            foreach (var convo in conversations)
            {
                if (convo.Type == ConvoType.Request)
                {
                    var convoTxt = convo.Text;
                    UserQuery = convoTxt;
                    UserQuery = UserQuery.Replace("[service]", serviceName);
                    if (caseName.Contains("free time slot"))
                    {
                        UserQuery = UserQuery.Replace("[Datetime]", freeDt);
                    }
                    if (caseName.Contains("booked time slot "))
                    {
                        UserQuery = UserQuery.Replace("[Datetime]", bookedDt);
                    }
                    UserQuery = UserQuery.Replace("[ConfirmDatetime]", freeDtConfirm);
                    UserQuery = UserQuery.Replace("[Provider]", providerName);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(string.Format("user: {0}", UserQuery));
                    ProcessUserQueryAsync(number).Wait();
                    WriteToFile(string.Format("user: {0}", UserQuery));
                }
                if (convo.Type == ConvoType.Response)
                {
                    var secondResponse = "";
                    var secondResponseDisplayText = "";
                    if (response != null)
                    {
                        var displayText = response.Result.Fulfillment.DisplayText;
                        convo.ActualResponse = BookaResponse;
                        Console.ForegroundColor = ConsoleColor.Green;
                        if (response == null || !convo.Text.Contains("<" + displayText + ">"))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                        }
                        var respText = response == null ? "Empty" : BookaResponse;
                        Console.Write($"Booka:{respText} \n");
                        WriteToFile("Booka: " + respText);

                        #region Second Confirmation
                        if (displayText == "Checking" || displayText == "CheckingConfirmation")
                        {
                            try
                            {
                                var latestMessage = GetLatestMessage(response.SessionId);
                                if (latestMessage != null)
                                {
                                    var reply = latestMessage.BookaReply;
                                    var foundResponse = false;
                                    if (!reply.Contains("Exception"))
                                    {
                                        if (!reply.Contains("ServiceAvailabilityChecked"))
                                        {
                                            while (!foundResponse)
                                            {
                                                latestMessage = GetLatestMessage(response.SessionId);
                                                reply = latestMessage.BookaReply;
                                                if (!reply.Contains("Exception"))
                                                {
                                                    
                                                    if (reply.Contains("SlotsAvailable") || reply.Contains("AlternateSlotsOnly") || reply.Contains("Booked") || reply.Contains("DuplicateAppointment") || reply.Contains("JustMissedAppointment"))
                                                    {
                                                        foundResponse = true;
                                                        GetSecondResponse(reply, latestMessage.ActionName, out secondResponse, out secondResponseDisplayText);
                                                    }                                                    
                                                }
                                                else
                                                {
                                                    foundResponse = true;
                                                    secondResponse = reply + latestMessage.ActionName;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            GetSecondResponse(reply, latestMessage.ActionName, out secondResponse, out secondResponseDisplayText);
                                        }
                                    }
                                    else
                                    {
                                        secondResponse = reply + latestMessage.ActionName;
                                    }
                                }
                                else
                                {
                                    secondResponse = "message not found";
                                    secondResponseDisplayText = "message not found";
                                }
                            }
                            catch (Exception e)
                            {
                                secondResponse = e.Message + e.StackTrace;
                                secondResponseDisplayText = "ErrorInDb";
                            }
                            if (!string.IsNullOrEmpty(secondResponse))
                            {
                                if (!convo.Text.Contains("<" + secondResponseDisplayText + ">"))
                                    Console.ForegroundColor = ConsoleColor.Red;
                                else
                                    Console.ForegroundColor = ConsoleColor.Green;
                                Console.Write($"Booka:{secondResponse} \n");
                                WriteToFile("Booka: " + secondResponse);
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Booka: Api.Ai not responding");
                        WriteToFile("Booka: Api.Ai not responding");
                    }
                }
            }
            if (response != null)
            {
                WriteToFile("AI Session:" + response.SessionId);
            }
            var breakLine = "----------------------------------------------------";
            WriteToFile(breakLine);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(breakLine);
            Console.Clear(); 
        }
        private static Message GetLatestMessage(string sessionId)
        {
            using (var chat = new ChatbookaEntities2())
            {
                return chat.Messages.Where(c => c.AiSessionId == sessionId).OrderByDescending(c => c.ModifieDateTime).FirstOrDefault();
            }
        }
        private static void GetSecondResponse(string reply, string actionName, out string secondResponse, out string secondResponseDisplayText)
        {
            secondResponse = "Response not available";
            secondResponseDisplayText = "Response not available";
            if (reply.Contains("SlotsAvailable") || reply.Contains("SlotsAvailable"))
            {
                var slots = reply.Split(',');
                freeDtConfirm = slots[1];
                var text = reply.Split(';');
                if (text.Length == 3)
                {
                    secondResponse = text[2];
                    secondResponseDisplayText = reply.Contains("SlotsAvailable") ? "SlotsAvailable" : "SlotsAvailable";
                }
            }
            if (reply.Contains("Booked") || reply.Contains("DuplicateAppointment") || reply.Contains("JustMissedAppointment"))
            {                
                var text = reply.Split(';');
                if (text.Length == 3)
                {
                    secondResponse = text[2];
                    secondResponseDisplayText = reply.Contains("DuplicateAppointment") ? "DuplicateAppointment" : reply.Contains("JustMissedAppointment") ? "JustMissedAppointment" : "Booked";
                }
            }
        }
        private static void ProcessCase(List<Conversation> conversations, string greetingReplacement, string AppmtRequest, string AppmtRequestConfirm, string caseName)
        {
            response = null;
            Initialise();
            var number = "+61400000000";
            if (caseName.Contains("New Customer"))
                number = "+61" + (new string(DateTime.Now.Ticks.ToString().Reverse().ToArray())).Substring(0, 8);

            foreach (var convo in conversations)
            {
                if (convo.Type == ConvoType.Request)
                {
                    var convoTxt = convo.Text;
                    UserQuery = convoTxt;
                    if (convoTxt.Contains("[Greeting]"))
                    {
                        UserQuery = convoTxt.Contains("[Greeting]") ? greetingReplacement : convoTxt;
                    }
                    if (convoTxt.Contains("[AppmtRequest]"))
                    {
                        UserQuery = convoTxt.Contains("[AppmtRequest]") ? AppmtRequest : convoTxt;
                        UserQuery = UserQuery.Replace("[service]", serviceName);
                        if (caseName.Contains("free time slot"))
                        {
                            UserQuery = UserQuery.Replace("[Datetime]", freeDt);
                        }
                        if (caseName.Contains("booked time slot "))
                        {
                            UserQuery = UserQuery.Replace("[Datetime]", bookedDt);
                        }
                    }
                    if (convoTxt.Contains("[AppmtRequestConfirm]"))
                    {
                        UserQuery = convoTxt.Contains("[AppmtRequestConfirm]") ? AppmtRequestConfirm : convoTxt;
                        UserQuery = UserQuery.Replace("[Datetime]", freeDtConfirm);
                    }
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(string.Format("user: {0}", UserQuery));
                    ProcessUserQueryAsync(number).Wait();
                    WriteToFile(string.Format("user: {0}", UserQuery));
                }
                if (convo.Type == ConvoType.Response)
                {
                    if (response.Result.Fulfillment.DisplayText == "SlotsAvailable" || response.Result.Fulfillment.DisplayText == "AlternateSlotsOnly")
                    {
                        freeDtConfirm = response.Result.Fulfillment.Speech.Split(',')[2];
                    }
                    convo.ActualResponse = BookaResponse;
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    if (response == null || !convo.Text.Contains("<" + response.Result.Fulfillment.DisplayText + ">"))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    var r = response == null ? "Empty" : BookaResponse;
                    Console.Write($"Booka:{r} \n");
                    WriteToFile(string.Format("Booka: Expected: {0}; Actual: {1}", convo.Text, r));
                    WriteToFile(response.Result.Fulfillment.DisplayText);
                }
            }
            var breakLine = "----------------------------------------------------";
            WriteToFile(breakLine);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(breakLine);
        }
        private static void WriteToFile(string line)
        {
            var e = new List<string>();
            e.Add(line);
            File.AppendAllLines(@"..\..\Data\results.txt", e);
        }
        #endregion
    }

    public class Expression
    {
        public List<Case> Cases { get; set; }
        public string Name { get; set; }
        public bool IsNewCustomer { get; set; }
    }

    public class Case
    {
        public string ExpressionName { get; set; }
        public string Name { get; set; }
        public List<Conversation> Conversations { get; set; }
    }

    public class Conversation
    {
        public ConvoType Type { get; set; }
        public string Text { get; set; }
        public string ActualResponse { get; set; }
    }
    public enum ConvoType { Request, Response }
}
