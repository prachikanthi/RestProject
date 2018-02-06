using NUnit.Framework;
using RestBasicProject.Authenticators;
using RestSharp;
using System.Net;

namespace RestBasicProject
{
    /// <summary>
    /// Examples of different Verification
    /// includes Post,Get,Basic authentication
    /// </summary>
    [TestFixture]
    class ValidateStatusCodes : BaseClass
    {
        /// <summary>
        /// This test script is for Status Code verification method
        /// </summary>
        [Test]
        public void validateStatusCodeForComments()
        {
            var request = new RestRequest("comments");
            IRestResponse response = client.Execute(request);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode,
                   string.Format("Expected status code {0} is not matching with actual {1}", HttpStatusCode.OK, response.StatusCode));
        }

        /// <summary>
        /// This test script is for GET method
        /// </summary>
        [Test]
        public void validateCommentsUsingID()
        {
            SupportClass support = new SupportClass();
            var request = new RestRequest("comments/11", Method.GET);
            request.AddUrlSegment("id", "11");

            Comments comment = support.Execute<Comments>(request, client);
            Assert.AreEqual(3, comment.postId);

        }
        /// <summary>
        /// This test script is for POST method
        /// </summary>
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
            string s = request.JsonSerializer.Serialize(pos);

            request.AddHeader("Accept", "application/json");
            request.Parameters.Clear();
            request.AddParameter("application/json", s, ParameterType.RequestBody);

            Posts posts = support.Execute<Posts>(request, client);
            Assert.AreEqual("bar", posts.body, string.Format("Expected body {0} is not matching with actual posted {1}", "bar", posts.body));

        }
        /// <summary>
        /// This test script for use of basic authentication
        /// </summary>
        [Test]
        public void Can_Authenticate_With_Basic_Auth()
        {
            AuthContext context = new BasicAuthContext(username, password);
            XAuthentication xas = new BasicAuthentication(context);

            var request = new RestRequest("/api/login", Method.POST);
            xas.Authenticate(client, request);
            var response = client.Execute(request);
            Assert.IsTrue(response.Content.Contains("authenticated"));
        }
    }
}
