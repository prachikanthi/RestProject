using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestBasicProject.Attributes
{
    public class ParameterNameAttribute : Attribute
    {
        public string Value { get; set; }

        public ParameterNameAttribute(string parameterName)
        {
            Value = parameterName;
        }
    }
}
