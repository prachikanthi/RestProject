using System;
using RestSharp;
using Newtonsoft.Json;

namespace RestBasicProject
{
    public class SupportClass
    {
        public T Execute<T>(RestRequest request, RestClient client) where T : new()
        {
            IRestResponse response = client.Execute<T>(request);

            var res = JsonConvert.DeserializeObject<T>(response.Content);

            if (response.ErrorException != null)
            {
                const string message = "Error retrieving response.  Check inner details for more info.";
                var twilioException = new ApplicationException(message, response.ErrorException);
                throw twilioException;
            }
            return res;

        }
    }
}
