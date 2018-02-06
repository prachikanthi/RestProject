using System;

namespace RestBasicProject.Authenticators
{
    public class BasicAuthContext : AuthContext
    {
        private string username;
        private string password;
        public BasicAuthContext(string userName,string password)
        {
            this.username = userName;
            this.password = password;
        }

        public string GetUsername()
        {
            return username;
        }

        public string GetPassword()
        {
            return password;
        }
    }
        
}

