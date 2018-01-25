using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestBasicProject.Requests
{
  public  interface IRestRequestProcessorBaseFactory
    {
        IBaseRestRequestProcessor Create(IRestBasicRequest request);
    }
}
