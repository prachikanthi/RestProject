using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestBasicProject.Attributes
{
    public class ParameterTypeAttribute : Attribute
    {
        public ParameterType Value { get; set; }

        public bool Ignored { get; set; }

        public ParameterTypeAttribute(ParameterType parameterType, bool ignored = false)
        {
            Value = parameterType;
            Ignored = ignored;
        }
    }
    }
