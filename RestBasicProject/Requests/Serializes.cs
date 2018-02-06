using Newtonsoft.Json;
using RestSharp.Serializers;
using System;

namespace RestBasicProject
{
    /// <summary>
    /// This Serializes class use for Serialization
    /// </summary>
    [Serializable]
    class Serializes : ISerializer
    {
        /// <summary>
        /// Constructor of Serialize class
        /// </summary>
        /// <param name="obj">Serialiazation of Object</param>
        /// <returns>serialized string</returns> 
        public string Serialize(object obj)
        {
            var str = JsonConvert.SerializeObject(obj, Formatting.None);
            return str;
        }
        /// <summary>
        /// property of ISerializer
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// property of ISerializer
        /// </summary>
        public string DateFormat { get; set; }

        /// <summary>
        /// property of ISerializer
        /// </summary>
        public string Namespace { get; set; }

        /// <summary>
        /// property of ISerializer
        /// </summary>
        public string RootElement { get; set; }

    }
}
