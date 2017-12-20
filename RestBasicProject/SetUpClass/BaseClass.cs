using NUnit.Framework;
using RestSharp;
using NUnit.Framework.Interfaces;
using Embarr.WebAPI.MediaTypes;

namespace RestBasicProject
{
    public class BaseClass
    {
        public static string ValidURI { get; set; }

        public RestClient client;

        [SetUp]
        public void TestInitialize()
        {
             ValidURI = "http://jsonplaceholder.typicode.com/";
            // ValidURI ="http://localhost:8080/";
            //ValidURI = "http://www.example.com/1/2";

            client = new RestClient();
            client.BaseUrl = new System.Uri(ValidURI);
            
            client.AddHandler("application/json",new Deserializes());
       
        }


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
