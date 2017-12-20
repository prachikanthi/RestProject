using RestBasicProject.Tools;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestBasicProject.Requests
{
    public abstract class RestRequestProcessorBase : IBaseRestRequestProcessor
    {
        protected IRestBasicRequest CurrentRequest;

        public virtual IRestRequest BuildRestRequest(IRestBasicRequest request)
        {
            CurrentRequest = request;

            IRestRequest restRequest = new RestRequest(request.Resource, request.Method)
            {
                RequestFormat = request.RequestFormat
            };

            return restRequest;
        }

        public virtual IRestRequest BuildRestRequest<T>(IRestBasicRequest<T> request) where T : new()
        {
            return BuildRestRequest((IRestBasicRequest)request);
        }

        public virtual void RestResponseHandler(IRestResponse response)
        {
        }
    }

    public abstract class EasyRestRequestProcessorBase<TRequest> : RestRequestProcessorBase, IBaseRestRequestProcessor<TRequest>
        where TRequest : IRestBasicRequest
    {
        protected TRequest CurrentTRequest;
        private IResourceParameterInjector _resourceParameterInjector;

        public virtual IResourceParameterInjector ResourceParameterInjector
        {
            get
            {
                return _resourceParameterInjector ?? (_resourceParameterInjector = new ResourceParameterInjector());
            }
            set
            {
                _resourceParameterInjector = value;
            }
        }

        public sealed override IRestRequest BuildRestRequest(IRestBasicRequest request)
        {
            var restRequest = base.BuildRestRequest(request);

            restRequest = AppendRequestDataToRestRequest(CurrentTRequest, restRequest);
            restRequest = ResourceParameterInjector.Inject(CurrentTRequest, restRequest);

            return restRequest;
        }

        public sealed override IRestRequest BuildRestRequest<T>(IRestBasicRequest<T> request)
        {
            CurrentTRequest = (TRequest)request;

            return base.BuildRestRequest(request);
        }

        public virtual IRestRequest AppendRequestDataToRestRequest(TRequest request, IRestRequest restRequest)
        {
            return restRequest;
        }
    }

    public abstract class RestRequestProcessorBase<TRequest, TResponse> : EasyRestRequestProcessorBase<TRequest>, IEasyRestRequestProcessor<TRequest, TResponse>
        where TRequest : IRestBasicRequest<TResponse>
        where TResponse : new()
    {
        public sealed override void RestResponseHandler(IRestResponse response)
        {
            RestResponseHandler((IRestResponse<TResponse>)response, CurrentTRequest);
        }

        public virtual void RestResponseHandler(IRestResponse<TResponse> response, TRequest request)
        {
        }
    }
}
    

