using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp.Extensions;
using RestSharp;
using RestSharp.Validation;
using RestSharp.Serializers;
using Newtonsoft.Json;

namespace RestBasicProject
{
    [Serializable]
    class Serializes : ISerializer
    {
        //private ISerializer serializer;

        //public Serializes(ISerializer serializer)
        //{
        //    this.serializer = serializer;
        //}

        public  string Serialize(object obj)
        {
            //  string str =JsonSerializer
            var str= JsonConvert.SerializeObject(obj, Formatting.None);
           // return JsonConvert.SerializeObject(obj, Formatting.None);

            //string str=  serializer.Serialize(obj);

            return str;
        }


        public string ContentType { get; set; }

        public string DateFormat { get; set; }

        public string Namespace { get; set; }

        public string RootElement { get; set; }

    }
}
