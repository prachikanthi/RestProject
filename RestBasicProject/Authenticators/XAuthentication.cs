using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestBasicProject.Authenticators
{
    /// <summary>
    /// This is abstract class for authentication
    /// </summary>
    public abstract class XAuthentication  : IAuthenticator
    {
        /// <summary>
        /// This is protected variable for authentication
        /// </summary>
        
        protected AuthContext authContext;


        /// <summary>
        /// This is method to set authcontext
        /// </summary>
        /// <param name="authContext"> authcontext</param>
        public XAuthentication(AuthContext authContext)
        {
            this.authContext = authContext;
        }

        /// <summary>
        /// This is abstract method of IAuthenticator
        /// </summary>
        /// <param name="client"> client</param>
        /// <param name="request"> request</param>
        public abstract void Authenticate(IRestClient client, IRestRequest request);

    }
}
