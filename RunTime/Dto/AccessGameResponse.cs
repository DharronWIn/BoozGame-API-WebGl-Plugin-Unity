using System;
using Newtonsoft.Json;
namespace BoozGameAPIProvider
{
    [Serializable]
    public class AccessGameResponse
    {
        public bool etat;
        public ResultDto result;
        public static AccessGameResponse FromJson(string json)
        {
            return JsonConvert.DeserializeObject<AccessGameResponse>(json);
        }
    }
}