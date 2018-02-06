using RestSharp.Authenticators;
using RestSharp;

namespace RestBasicProject.Authenticators
{
    /// <summary>
    /// This is BasicAuthentication Class inherits XAuthentication
    /// </summary>
    public class BasicAuthentication : XAuthentication
    {
        /// <summary>
        /// This is BasicAuthentication constructor initializes authcontext
        /// </summary>
        public BasicAuthentication(AuthContext authContext) : base(authContext)
        {

        }

        /// <summary>
        /// This is Authenticate method which authenticate URL by passing user name and password
        /// </summary>
        /// <param name="client">client</param>
        /// <param name="request">request</param>
        /// <returns>Auth header if authentication gets passed</returns>
        public override void Authenticate(IRestClient client, IRestRequest request)
        {
            request.AddParameter("username", ((BasicAuthContext)authContext).GetUsername());
            request.AddParameter("password", ((BasicAuthContext)authContext).GetPassword());

            client.Authenticator = new HttpBasicAuthenticator(((BasicAuthContext)authContext).GetUsername(), ((BasicAuthContext)authContext).GetPassword());
        }

    }
}
