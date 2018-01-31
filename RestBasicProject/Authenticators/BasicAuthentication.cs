using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace RestBasicProject.Authenticators
{
    public class BasicAuthentication : IAuthentication
    {
        private BasicAuthContext context;

        public void Authenticate(IRestClient client, IRestRequest request)
        {
            request.AddParameter("username", context.getUsername());
            request.AddParameter("password", context.getPassword());

            client.Authenticator = new HttpBasicAuthenticator(context.getUsername(), context.getPassword());
        }

        public void setAuthContext(AuthContext context)
        {
          this.context = (BasicAuthContext)context;
        }
    }
}
