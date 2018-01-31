using RestBasicProject.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RestBasicProject.Authenticators
{
    public class BasicAuthContext : AuthContext
    {
        AuthContext context;
        private string username;
        private string password;
        public BasicAuthContext(string userName,string password)
        {
            this.username = userName;
            this.password = password;
        }

        public String getUsername()
        {
            return username;
        }

        public String getPassword()
        {
            return password;
        }
    }
        
}

