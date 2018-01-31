using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestBasicProject.Authenticators
{
    public interface IAuthentication :IAuthenticator
    {
        // override void Authenticate(IRestClient client, IRestRequest request);
         void setAuthContext(AuthContext context);


    }
}
