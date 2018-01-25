using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestBasicProject.Authenticators
{
    public class BasicAuthContext : AuthContext
    {
        private String username;
        private String password;
        public BasicAuthContext(String username, String password)
        {
            this.username = username;
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
