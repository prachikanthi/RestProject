using System;

namespace RestBasicProject.Authenticators
{
    /// <summary>
    /// This is BasicAuthContext Class to get credentials and also inherit AuthContext class
    /// </summary>
    public class BasicAuthContext : AuthContext
    {
        private string username;
        private string password;

        /// <summary>
        /// This is BasicAuthContext constructor to get credentials
        /// </summary>
        /// <param name="password">password</param>
        /// <param name="userName">User name</param>
        public BasicAuthContext(string userName,string password)
        {
            this.username = userName;
            this.password = password;
        }

        /// <summary>
        /// This method Class to get user name
        /// </summary>
        public string GetUsername()
        {
            return username;
        }

        /// <summary>
        /// This method Class to get password
        /// </summary>
        public string GetPassword()
        {
            return password;
        }
    }
        
}

