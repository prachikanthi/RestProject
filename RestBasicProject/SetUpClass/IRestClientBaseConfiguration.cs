using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestBasicProject.SetUpClass
{
   public interface IRestClientBaseConfiguration
    {
        string BaseUrl { get; }

        IAuthenticator Authenticator { get; }
    }
}
