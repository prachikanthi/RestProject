using System;
using RestSharp;
using Newtonsoft.Json;

namespace RestBasicProject
{
    /// <summary>
    /// This is support class contains execute method 
    /// </summary>
    public class SupportClass
    {
        /// <summary>
        /// This method executes request also desrializes the response
        /// </summary>
        /// <typeparam name="T">RestSharp parameters</typeparam>
        /// <param name="request">Rest Client</param>
        /// <param name="client">Rest Request</param>
        /// <returns>Returns Response of request if passes the request</returns>
       
        public T Execute<T>(RestRequest request, RestClient client) where T : new()
        {
            IRestResponse response = client.Execute<T>(request);

            var responseData = JsonConvert.DeserializeObject<T>(response.Content);

            if (response.ErrorException != null)
            {
                const string message = "Error retrieving response.  Check inner details for more info.";
                var twilioException = new ApplicationException(message, response.ErrorException);
                throw twilioException;
            }
            return responseData;

        }
    }
}
