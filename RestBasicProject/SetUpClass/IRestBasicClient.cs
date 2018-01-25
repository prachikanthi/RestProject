using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using RestBasicProject.Requests;

namespace RestBasicProject.SetUpClass
{
   public interface IRestBasicClient
    {
        IRestResponse Execute(IRestBasicRequest request);

        T Execute<T>(IRestBasicRequest<T> request) where T : new();
    }
}
