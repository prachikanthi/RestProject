using RestBasicProject.Requests;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestBasicProject.Tools
{
   public interface IResourceParameterInjector
    {
        bool IgnoreNull { get; set; }

        IRestRequest Inject<T>(T request, IRestRequest restRequest) where T : IRestBasicRequest;
    }
}
