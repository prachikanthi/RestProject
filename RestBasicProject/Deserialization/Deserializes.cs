using RestSharp;
using RestSharp.Deserializers;
using RestSharp.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace RestBasicProject
{
    /// <summary>
    /// This is deserialization Class
    /// </summary>
    public class Deserializes : IDeserializer
    {
        /// <summary>
        /// This is variable to set culture
        /// </summary>
        public CultureInfo Culture { get; set; }

        /// <summary>
        /// Constructor of Deserializes class
        /// </summary>
        public Deserializes()
        {
            Culture = CultureInfo.InvariantCulture;
        }

        /// <summary>
        /// This is variable to set user id of Posts
        /// </summary>
        public string ContentType
        {
            get { return "application/json"; } // Probably used for Serialization?
            set { }
        }

        /// <summary>
        /// This is variable to data format of IDeserializer
        /// </summary>
        public string DateFormat { get; set; }

        /// <summary>
        /// This is variable to find root element of IDeserializer
        /// </summary>
        public string RootElement { get; set; }

        /// <summary>
        /// This is variable to get namespace of IDeserializer
        /// </summary>
        public string Namespace { get; set; }

        /// <summary>
        /// Desrializes response by finding roor of response content
        /// </summary>
        /// <typeparam name="T">Rest sharp parameters types-RestResponse</typeparam>
        /// <param name="response"></param>
        /// <returns>return response by deserializing JSON response into proper return type</returns>
        T IDeserializer.Deserialize<T>(IRestResponse response)
        {
            var json = FindRoot(response.Content);

            return (T)ConvertValue(typeof(T).GetTypeInfo(), json);
        }
        /// <summary>
        /// Returns root if response contains it
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private object FindRoot(string content)
        {
            object json = SimpleJson.SimpleJson.DeserializeObject(content);
            object result;
            if (!this.RootElement.HasValue())
                return json;

            IDictionary<string, object> dictionary = json as IDictionary<string, object>;

            return (dictionary.TryGetValue(this.RootElement, out result)) ? result : json;


        }
        /// <summary>
        /// Json response converts to its expected data type and culture
        /// </summary>
        /// <param name="typeInfo"></param>
        /// <param name="value"></param>
        /// <returns></returns>
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
            }

            return null;
        }

        private object CreateAndMap(Type type, object element)
        {
            object instance = Activator.CreateInstance(type);

            IDictionary<string, object> dd = element as IDictionary<string, object>;
            Map(instance, dd);

            return instance;
        }

        /// <summary>
        /// Gets type of response and mapping it with its expected data type
        /// </summary>
        /// <param name="target"></param>
        /// <param name="data"></param>
        /// <returns></returns>

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
