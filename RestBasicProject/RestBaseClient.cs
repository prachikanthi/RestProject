using RestBasicProject.Requests;
using RestBasicProject.SetUpClass;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RestBasicProject
{
    
        public class RestBaseClient : IRestBasicClient
        {
            private readonly IRestClientBaseConfiguration _configuration;
            private readonly IRestRequestProcessorBaseFactory _requestProcessorFactory;
            private readonly IRestClient _restClient;

            public RestBaseClient(IRestClientBaseConfiguration configuration,
                IRestRequestProcessorBaseFactory requestProcessorFactory,
                IRestClient restClient)
            {
                this._configuration = configuration;
                this._requestProcessorFactory = requestProcessorFactory;
                this._restClient = restClient;

                SetupRestClient();
            }

            private void SetupRestClient()
            {
                _restClient.BaseUrl = new Uri(_configuration.BaseUrl);
                _restClient.Authenticator = _configuration.Authenticator;
            }

            public IRestResponse Execute(IRestBasicRequest request)
            {
                var requestProcessor = _requestProcessorFactory.Create(request);
                var restRequest = requestProcessor.BuildRestRequest(request);

                var response = _restClient.Execute(restRequest);

                DefualtResponseHandler(response);
                requestProcessor.RestResponseHandler(response);

                return response;
            }

            public T Execute<T>(IRestBasicRequest<T> request) where T : new()
            {
                var requestProcessor = _requestProcessorFactory.Create(request);
                var restRequest = requestProcessor.BuildRestRequest(request);

                var response = _restClient.Execute<T>(restRequest);

                DefualtResponseHandler(response);
                requestProcessor.RestResponseHandler(response);

                return response.Data;
            }

            private static void DefualtResponseHandler(IRestResponse response)
            {
                if (response == null)
                {
                   // throw new EasyRestException("Response is null for unknow reason.");
                }

                if (response.ErrorException != null)
                {
                    //throw new EasyRestException(response.ErrorMessage, response.ErrorException);
                }

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        return;
                    case HttpStatusCode.NotFound:
                        return;
                       // throw new EasyRestException(string.Format("Not found endpoint: {0}",
                        //    response.Request.Resource));
                    case HttpStatusCode.Forbidden:
                        return;
                       // throw new EasyRestException(string.Format("Not authorized for endpoint: {0}",
                        //    response.Request.Resource));
                }
            }

         
        
    }
}
