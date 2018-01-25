using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestBasicProject.Authenticators
{
    public class BasicAuthentication : IAuthentication
    {
        private BasicAuthContext context;
        public void setAuthContext(AuthContext context)
        {
           // this.context = (HttpBasicAuthenticator)context;
        }
    }
}
