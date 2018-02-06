using Newtonsoft.Json;
using RestSharp.Serializers;
using System;

namespace RestBasicProject
{
    [Serializable]
    class Serializes : ISerializer
    {
        public string Serialize(object obj)
        {
            var str = JsonConvert.SerializeObject(obj, Formatting.None);
            return str;
        }

        public string ContentType { get; set; }

        public string DateFormat { get; set; }

        public string Namespace { get; set; }

        public string RootElement { get; set; }

    }
}
