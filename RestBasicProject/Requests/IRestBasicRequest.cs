using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestBasicProject.Requests
{
   public interface IRestBasicRequest
    {
        string Resource { get; }

        Method Method { get; }

        DataFormat RequestFormat { get; }
    }

    public interface IRestBasicRequest<T> : IRestBasicRequest where T : new()
    {
    }
}

