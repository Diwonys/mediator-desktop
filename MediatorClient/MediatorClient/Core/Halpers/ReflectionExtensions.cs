using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediatorClient.Core.Halpers
{
    static class ReflectionExtensions
    {
        public static object GetPropertyValue(this object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }
    }
}
