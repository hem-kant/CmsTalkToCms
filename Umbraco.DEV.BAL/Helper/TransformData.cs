using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Umbraco.DEV.BAL.Helper
{
   public static class TransformData
    {
        public static Object GetObject(this Dictionary<string, string> dict, Type type)
        {
            var obj = Activator.CreateInstance(type);

            foreach (var kv in dict)
            {
                var prop = type.GetProperty(kv.Key);
                if (prop == null) continue;

                object value = kv.Value;
                if (value is Dictionary<string, object>)
                {
                    value = GetObject((Dictionary<string, string>)value, prop.PropertyType); // <= This line
                }

                prop.SetValue(obj, value, null);
            }
            return obj;
        }
    }
}
