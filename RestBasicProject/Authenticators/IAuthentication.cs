using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestBasicProject.Authenticators
{
    public interface IAuthentication
    {
        void setAuthContext(AuthContext context);
    }
}
