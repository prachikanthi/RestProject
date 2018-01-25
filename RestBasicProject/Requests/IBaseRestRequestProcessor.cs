using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestBasicProject.Requests
{
  public interface IBaseRestRequestProcessor
    {
        IRestRequest BuildRestRequest(IRestBasicRequest request);

        IRestRequest BuildRestRequest<T>(IRestBasicRequest<T> request) where T : new();

        void RestResponseHandler(IRestResponse response);
    }
    
    public interface IBaseRestRequestProcessor<in TRequest> : IBaseRestRequestProcessor
        where TRequest : IRestBasicRequest
    {
        IRestRequest AppendRequestDataToRestRequest(TRequest request, IRestRequest restRequest);
    }

    public interface IEasyRestRequestProcessor<in TRequest, TResponse> : IBaseRestRequestProcessor<TRequest>
        where TRequest : IRestBasicRequest<TResponse>
        where TResponse : new()
    {
        void RestResponseHandler(IRestResponse<TResponse> response, TRequest request);
    }
}
