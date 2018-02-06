using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestBasicProject.Authenticators
{
    public abstract class XAuthentication  : IAuthenticator
    {
        protected AuthContext authContext;

        public XAuthentication(AuthContext authContext)
        {
            this.authContext = authContext;
        }

        public abstract void Authenticate(IRestClient client, IRestRequest request);

    }
}
