using RestBasicProject.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestBasicProject.Managers
{
    public class ApiContext
    {
        IAuthentication strategy;

        public IAuthentication getStrategy()
        {
            return strategy;
        }

        public void setStrategy(IAuthentication strategy)
        {
            this.strategy = strategy;
        }
    }
}

