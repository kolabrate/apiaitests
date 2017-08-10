using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ApiAiSDK;
using ApiAiSDK.Model;
using System.IO;

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

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.Read();
                
            }

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


        private static async Task ProcessUserQueryAsync()
        {
            try
            {
                var response = GetResponse(UserQuery).Result;
                if (response.IsError == false)
                {
                    BookaResponse = response.Result.Fulfillment.Speech;
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

        private static async Task<AIResponse> GetResponse(string userQuery)
        {
            var request = new AIRequest(userQuery);
            request.OriginalRequest = PopulateOriginalRequest();
            var response = await dataService.RequestAsync(request);
           
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
        private static OriginalRequest PopulateOriginalRequest()
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
                    From = "+61468492337", //a temp phone number that represents its from console app.
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
        private static bool IsError { get; set; }

        public static void AllConversations()
        {
            var greetingVariations = new List<string>();
            var exits = Directory.Exists(@"..\Data\");
            var list = File.ReadAllLines(@"..\..\Data\conversations.txt");
            var expressions = new List<Expression>();
            Expression currentExpr = null;
            Case currentCase = null;
            Conversation currentConvo = null;

            #region Build Expressions
            foreach (var str in list)
            {
                if (str.Contains("Name=Greeting;"))
                {
                    var val = str.Replace("variations: Name=Greeting; Values=", "");
                    var values = val.Split(',');
                    foreach (var greetingVal in values)
                    {
                        greetingVariations.Add(greetingVal);
                    }
                }
                if (str.Contains("Expression:"))
                {
                    var exprName = str.Replace("Expression:", "");
                    currentExpr = new Expression()
                    {
                        Name = exprName,
                        Cases = new List<Case>()
                    };
                    expressions.Add(currentExpr);
                }
                if (str.Contains("Case:"))
                {
                    var caseName = str.Replace("Case:", "");
                    currentCase = new Case()
                    {
                        Name = caseName,
                        Conversations = new List<Conversation>()
                    };
                    currentExpr.Cases.Add(currentCase);
                }
                if (str.Contains("Customer:") || str.Contains("Booka:"))
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
            #endregion

            #region Process Expressions
            foreach (var expression in expressions)
            {
                foreach (var caseItem in expression.Cases)
                {

                    if (caseItem.Conversations.Exists(x => x.Text.Contains("[Greeting]")))
                    {
                        foreach (var greetingVariation in greetingVariations)
                        {
                            ProcessCase(caseItem.Conversations, greetingVariation);
                        }
                    }
                    else
                    {
                        ProcessCase(caseItem.Conversations, null);
                    }
                }
            }



            #endregion

            Console.ReadLine();
        }
        private static void ProcessCase(List<Conversation> conversations, string greetingReplacement)
        {
            Initialise();

            foreach (var convo in conversations)
            {
                if (convo.Type == ConvoType.Request)
                {
                    UserQuery = convo.Text.Contains("[Greeting]") ? greetingReplacement : convo.Text;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(string.Format("user: {0}",UserQuery));
                    ProcessUserQueryAsync().Wait();
                }
                if (convo.Type == ConvoType.Response)
                {
                    convo.ActualResponse = BookaResponse;
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.Write($"Booka:{BookaResponse} \n");
                }
            }
        }
        #endregion
    }

    public class Expression
    {
        public List<Case> Cases { get; set; }
        public string Name { get; set; }
    }

    public class Case
    {
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
