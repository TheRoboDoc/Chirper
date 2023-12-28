using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Chirper.Data
{
    public static class Converter
    {
        public static T? JsonToData<T>(string jsonString)
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        public static string? DataToJson(object data)
        {
            Type fromType = data.GetType();

            return JsonConvert.SerializeObject(value: data, type: fromType, null);
        }
    }
}
