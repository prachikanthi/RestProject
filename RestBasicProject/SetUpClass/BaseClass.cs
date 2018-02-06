using NUnit.Framework;
using NUnit.Framework.Interfaces;
using RestSharp;
using System.Configuration;

namespace RestBasicProject
{
    /// <summary>
    /// This Class will work as base class for all other classes
    /// </summary>
    public class BaseClass
    {
        /// <summary>
        /// This variable uses as application URL
        /// </summary>
        public static string ValidURI { get; set; }

        /// <summary>
        /// This variable uses as Rest Client
        /// </summary>
        public RestClient client;

        /// <summary>
        /// This variable uses as User Name
        /// </summary>
        public string username;

        /// <summary>
        /// This variable uses as Password
        /// </summary>
        public string password;

        /// <summary>
        /// This is Test Initialization method calls before every test gets started
        /// Setting Client and URL with deserialization handler for JSON type
        /// </summary>

        [SetUp]
        public void TestInitialize()
        {
            //ValidURI = "http://jsonplaceholder.typicode.com/";
            //ValidURI = "http://www.newsblur.com";
            ValidURI = ConfigurationManager.AppSettings["ApplicationUrl"];
            username= ConfigurationManager.AppSettings["UserName"];
            password= ConfigurationManager.AppSettings["Password"];
            client = new RestClient();
            client.BaseUrl = new System.Uri(ValidURI);
            client.AddHandler("application/json", new Deserializes());
        }

        /// <summary>
        /// This is clean up method shows status of test script
        /// </summary>
        /// <returns>Returns on the basis of test case status</returns>
        
        [TearDown]
        public void TestCleanUp()

        {
            var status = TestContext.CurrentContext.Result.Outcome.Status;
            var stackTrace = string.IsNullOrEmpty(TestContext.CurrentContext.Result.StackTrace) ? "" : string.Format("{0}", TestContext.CurrentContext.Result.StackTrace);

            switch (status)
            {
                case TestStatus.Failed:
                    Assert.IsTrue(false, "failed");
                    break;
                case TestStatus.Inconclusive:
                    Assert.IsTrue(false, "inconclusive");
                    break;
                case TestStatus.Skipped:
                    Assert.IsTrue(false, "Skipped");
                    break;
                default:
                    Assert.IsTrue(true, "passed");
                    break;
            }
        }
    }
}
