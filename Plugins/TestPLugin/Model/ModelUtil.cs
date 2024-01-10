using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPlugin.Model
{
    internal static class ModelUtile
    {
        public static T ConvertObject<T>(object asObject) where T : new()
        {
            //此方法将object对象转换为json字符
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(asObject);
            //再将json字符转换为实体对象
            var t = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
            return t;
        }
    }
}
