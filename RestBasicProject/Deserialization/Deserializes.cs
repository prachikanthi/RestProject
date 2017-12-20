using System;
using System.Collections.Generic;
using System.Linq;
using RestSharp;
using RestSharp.Deserializers;
using System.Globalization;
using RestSharp.Extensions;
using System.Reflection;
using System.Collections;



namespace RestBasicProject
{
    public class Deserializes : IDeserializer
    {
       
        // private IDeserializer serializer;

        //public Deserializes(IDeserializer serializer)
        //{
        //    this.serializer = serializer;
        //}

        //public T Deserialize<T>(RestSharp.IRestResponse response)
        //{
        //    var content = response.Content;

        //    using (var stringReader = new StringReader(content))
        //    {
        //        using (var jsonTextReader = new  (stringReader))
        //        {
        //            return serializer.Deserialize<T>(jsonTextReader);
        //        }
        //    }
        //}

        public CultureInfo Culture { get; set; }

        public Deserializes()
        {
            Culture = CultureInfo.InvariantCulture;
        }

        public string ContentType
        {
            get { return "application/json"; } // Probably used for Serialization?
            set { }
        }

        //T IDeserializer.Deserialize<T>(IRestResponse response)
        //{
        //    return JsonConvert.DeserializeObject<T>(response.Content);
        //}

        public string DateFormat { get; set; }

        public string RootElement { get; set; }

        public string Namespace { get; set; }

        T IDeserializer.Deserialize<T>(IRestResponse response)
        {
           // IEnumerable < Deserializes > object = jsonSerializer.Deserialize<IEnumerable<Deserializes>>(json);

            var json = FindRoot(response.Content);

            return (T)ConvertValue(typeof(T).GetTypeInfo(), json);
        }

        private object FindRoot(string content)
        {
            //IDictionary<string, object> dictionary = new Dictionary<string, object>();

            ////  object json = SimpleJson.SimpleJson.DeserializeObject(content);


            // JObject json= JObject.Parse(content);
            //var root = json.Root;
            //if (!RootElement.HasValue()) return json;

            //if (!(json is IDictionary<string, object>))
            //    return json;

            //return root;
            // return dictionary.TryGetValue(RootElement,out object result) ? result : json;

            //object json = SimpleJson.SimpleJson.DeserializeObject(content);
            //var result =new object();
            //IDictionary<string, object> dictionary = new Dictionary<string, object>();
            //if (!RootElement.HasValue()) return json;

            //if (!(json is IDictionary<string, object>))
            //   return json;

            //return dictionary.TryGetValue(RootElement, out result) ? result : json;
           // IDictionary<string, Dictionary<string, object>> json = SimpleJson.SimpleJson.DeserializeObject<Dictionary<string, Dictionary<string, object>>>(content);
             object json = SimpleJson.SimpleJson.DeserializeObject(content);
            // RestSharp.Deserializers.JsonDeserializer json = new RestSharp.Deserializers.JsonDeserializer();
            //  var rootObject = deserial.Deserialize<T>(content);
            object result;
            // dynamic json = JValue.Parse(content);
            // var json = JsonConvert.DeserializeObject<List<object>>(content);
            if (!this.RootElement.HasValue())
                return json;

            IDictionary<string, object> dictionary = json as IDictionary<string, object>;

            //if (dictionary != null)
            //{

            return (dictionary.TryGetValue(this.RootElement, out result))? result : json;
            
             //   return result;
            
        

           // return dictionary.TryGetValue(this.RootElement, out result);
           
        }

        private object ConvertValue(TypeInfo typeInfo, object value)
        {
            string stringValue = Convert.ToString(value, Culture);

            // check for nullable and extract underlying type
            if (typeInfo.IsGenericType && typeInfo.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                // Since the type is nullable and no value is provided return null
                if (string.IsNullOrEmpty(stringValue))
                {
                    return null;
                }

                typeInfo = typeInfo.GetGenericArguments()[0].GetTypeInfo();
            }

            if (typeInfo.AsType() == typeof(object))
            {
                if (value == null)
                {
                    return null;
                }
                typeInfo = value.GetType().GetTypeInfo();
            }

            var type = typeInfo.AsType();
            if (typeInfo.IsPrimitive)
            {
                return value.ChangeType(type, Culture);
            }

            if (typeInfo.IsEnum)
            {
                return type.FindEnumValue(stringValue, Culture);
            }

            if (type == typeof(Uri))
            {
                return new Uri(stringValue, UriKind.RelativeOrAbsolute);
            }

            if (type == typeof(string))
            {
                return stringValue;
            }

            if (type == typeof(DateTime) || type == typeof(DateTimeOffset))
            {
                DateTime dt;

                if (DateFormat.HasValue())
                {
                    dt = DateTime.ParseExact(stringValue, DateFormat, Culture,
                        DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
                }
                else
                {
                    // try parsing instead
                    dt = stringValue.ParseJsonDate(this.Culture);
                }

            }
            else if (type.GetTypeInfo().IsGenericType)
            {
                Type genericTypeDef = type.GetGenericTypeDefinition();

                if (genericTypeDef == typeof(IEnumerable<>))
                {
                    Type itemType = typeInfo.GetGenericArguments()[0];
                    Type listType = typeof(List<>).MakeGenericType(itemType);
                    return BuildList(listType, value);
                }

                if (genericTypeDef == typeof(List<>))
                {
                    return BuildList(type, value);
                }

                if (genericTypeDef == typeof(Dictionary<,>))
                {
                    return BuildDictionary(type, value);
                }

                // nested property classes
                return CreateAndMap(type, value);
            }
           
            else
            {

                // nested property classes
                 return CreateAndMap(type, value);
                //return
                //     Mapper.Map(type, value);
            }

            return null;
        }

        private object CreateAndMap(Type type, object element)
        {
            object instance = Activator.CreateInstance(type);
           

            IDictionary<string, object> dd = element as IDictionary<string, object>;

            Map(instance,dd);

            return instance;
        }

        private object Map(object target, IDictionary<string, object> data)
        {
            var objType = target.GetType().GetTypeInfo();
            var props = objType.GetProperties()
                .Where(p => p.CanWrite)
                .ToList();

            foreach (var prop in props)
            {
                string name;
                var type = prop.PropertyType.GetTypeInfo();
                var attributes = prop.GetCustomAttributes(typeof(DeserializeAsAttribute), false);

                if (attributes.Any())
                {
                    var attribute = (DeserializeAsAttribute)attributes.First();
                    name = attribute.Name;
                }
                else
                {
                    name = prop.Name;
                }

                object value = null;
                if (!data.TryGetValue(name, out value))
                {
                    string[] parts = name.Split('.');
                    IDictionary<string, object> currentData = data;

                    for (int i = 0; i < parts.Length; ++i)
                    {
                        string actualName = parts[i].GetNameVariants(this.Culture)
                            .FirstOrDefault(currentData.ContainsKey);

                        if (actualName == null)
                        {
                            break;
                        }

                        if (i == parts.Length - 1)
                        {
                            value = currentData[actualName];
                        }
                        else
                        {
                            currentData = (IDictionary<string, object>)currentData[actualName];
                        }
                    }
                }

                if (value != null)
                {
                    prop.SetValue(target, ConvertValue(type, value), null);
                }
            }

            return target;
        }

        private IDictionary BuildDictionary(Type type, object parent)
        {
            IDictionary dict = (IDictionary)Activator.CreateInstance(type);
            Type keyType = type.GetTypeInfo().GetGenericArguments()[0];
            Type valueType = type.GetTypeInfo().GetGenericArguments()[1];

            foreach (KeyValuePair<string, object> child in (IDictionary<string, object>)parent)
            {
                object key = keyType != typeof(string)
                    ? Convert.ChangeType(child.Key, keyType, CultureInfo.InvariantCulture)
                    : child.Key;

                object item;

                if (valueType.GetTypeInfo().IsGenericType &&
                    valueType.GetTypeInfo().GetGenericTypeDefinition() == typeof(List<>))
                {
                    item = BuildList(valueType, child.Value);
                }
                else
                {
                    item = ConvertValue(valueType.GetTypeInfo(), child.Value);
                }

                dict.Add(key, item);
            }

            return dict;
        }

        private IList BuildList(Type type, object parent)
        {
            IList list = (IList)Activator.CreateInstance(type);
            Type listType = type.GetTypeInfo().GetInterfaces()
                .First
                    (x => x.GetTypeInfo().IsGenericType && x.GetGenericTypeDefinition() == typeof(IList<>));
            Type itemType = listType.GetTypeInfo().GetGenericArguments()[0];

            if (parent is IList)
            {
                IList list1 = (IList)parent;
                foreach (object element in list1)
                {
                    if (itemType.GetTypeInfo().IsPrimitive)
                    {
                        object item = ConvertValue(itemType.GetTypeInfo(), element);

                        list.Add(item);
                    }
                    else if (itemType == typeof(string))
                    {
                        if (element == null)
                        {
                            list.Add(null);
                            continue;
                        }

                        list.Add(element.ToString());
                    }
                    else
                    {
                        if (element == null)
                        {
                            list.Add(null);
                            continue;
                        }

                        object item = ConvertValue(itemType.GetTypeInfo(), element);

                        list.Add(item);
                    }
                }
            }
            else
            {
                list.Add(ConvertValue(itemType.GetTypeInfo(), parent));
            }

            return list;
        }

    }
} 
