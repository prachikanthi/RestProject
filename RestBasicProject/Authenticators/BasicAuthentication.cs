using RestSharp.Authenticators;
using RestSharp;

namespace RestBasicProject.Authenticators
{
    public class BasicAuthentication : XAuthentication
    {
        public BasicAuthentication(AuthContext authContext) : base(authContext)
        {

        }

        public override void Authenticate(IRestClient client, IRestRequest request)
        {
            request.AddParameter("username", ((BasicAuthContext)authContext).GetUsername());
            request.AddParameter("password", ((BasicAuthContext)authContext).GetPassword());

            client.Authenticator = new HttpBasicAuthenticator(((BasicAuthContext)authContext).GetUsername(), ((BasicAuthContext)authContext).GetPassword());
        }

    }
}
