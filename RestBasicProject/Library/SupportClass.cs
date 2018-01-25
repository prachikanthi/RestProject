using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using RestSharp;
using RestSharp.Deserializers;
using Newtonsoft.Json;


namespace RestBasicProject
{
    public class SupportClass
    {
        
        public T Execute<T>(RestRequest request,RestClient client) where T:new()
        {
            IRestResponse response = client.Execute<T>(request);

            //   var response = client.Execute<dynamic>(new RestRequest("http://dummy/users/42"));

            var res = JsonConvert.DeserializeObject<T>(response.Content);

            // var r = JsonConvert.DeserializeObject(response.ToString());
            if (response.ErrorException!=null)
            {
                const string message = "Error retrieving response.  Check inner details for more info.";
                var twilioException = new ApplicationException(message, response.ErrorException);
                throw twilioException;
            }
            return res;
        
        }
    }
}
