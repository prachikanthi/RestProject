using System;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Web;
using NUnit.Framework;
using RestSharp.Authenticators;
using RestBasicProject.Helpers;
using RestSharp;
using System.Threading;
//using RestSharp.IntegrationTests.Helpers;

namespace RestBasicProject
{
    [TestFixture]
    class ValidateStatusCodes : BaseClass
    {

        private static void UsernamePasswordEchoHandler(HttpListenerContext context)
        {
            var header = context.Request.Headers["Authorization"];
            var parts = Encoding.ASCII.GetString(Convert.FromBase64String(header.Substring("Basic ".Length)))
                .Split(':');


            string responseString = string.Join(" | ", parts);
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
            // Get a response stream and write the response to it.
           // response.ContentLength64 = buffer.Length;
            System.IO.Stream output = context.Response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            Thread.Sleep(4000);
           // context.Response.OutputStream.WriteStringUtf8(string.Join("|", parts));
        }

        [Test]
        public void validateStatusCodeForComments()
        {
            var request = new RestRequest("comments");
            IRestResponse response = client.Execute(request);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode,
                   string.Format("Expected status code {0} is not matching with actual {1}", HttpStatusCode.OK, response.StatusCode));
        }

        [Test]
        public void validateCommentsUsingID()
        {
            SupportClass support = new SupportClass();

            var request = new RestRequest("comments/11",Method.GET);
            request.AddUrlSegment("id", "11");

            Comments comment = support.Execute<Comments>(request, client);
            Assert.AreEqual(3, comment.postId);

        }
        [Test]
        public void validateCommentsUsingIDPost()
        {
            SupportClass support = new SupportClass();
           

            
              var request = new RestRequest("/posts", Method.POST);


            Posts pos = new Posts();
            pos.title = "New";
            pos.userId = 1;
            pos.body = "bar";
            request.RequestFormat = DataFormat.Json;
            request.JsonSerializer = new Serializes();
            string s=request.JsonSerializer.Serialize(pos);
            // request.AddBody(pos);

            // var request = new RestRequest();
            // request.Method = Method.POST;
            //request.AddHeader("Accept", "application/json");
            //request.Parameters.Clear();
            //request.AddParameter("application/json; charset=utf-8", ParameterType.RequestBody);
            //request.AddParameter(s);
            //request.AddParameter("body", "bar");
            //request.AddParameter("userID", 1);
            request.AddHeader("Accept", "application/json");
            request.Parameters.Clear();
            request.AddParameter("application/json", s, ParameterType.RequestBody);
            //request.AddUrlSegment("id", "11");

            Posts posts = support.Execute<Posts>(request, client);
            Assert.AreEqual("bar", posts.body, string.Format("Expected body {0} is not matching with actual posted {1}", "bar", posts.body));
            //  Assert.AreEqual(3, comment.postId);

        }


        [Test]
        public void Can_Authenticate_With_Basic_Http_Auth()
        {
            //var baseUrl = new Uri("http://localhost:8888/");

            //SupportClass support = new SupportClass();
            using (SimpleServer.Create(client.BaseUrl.AbsoluteUri, UsernamePasswordEchoHandler))
            {
                var client = new RestClient(ValidURI)
                {
                    Authenticator = new HttpBasicAuthenticator("testuser", "testpassword")
                };
                var request = new RestRequest("test");
                var response = client.Execute(request);

                Assert.IsTrue(response.Content.Contains("testuser | testpassword") );
            }
        }


       
    }

}
