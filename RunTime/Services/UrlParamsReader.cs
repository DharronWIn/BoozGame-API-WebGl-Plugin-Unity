using UnityEngine;
using System.Collections.Generic;

namespace BoozGameAPIProvider
{
    public static class UrlParamsReader 
    {
        public static Dictionary<string, string> GetQueryParameters(string url)
            {
                Dictionary<string, string> result = new Dictionary<string, string>();

                if (url.Contains("?"))
                {
                    string[] parts = url.Split('?');
                    if (parts.Length > 1)
                    {
                        string query = parts[1];
                        string[] parameters = query.Split('&');

                        foreach (string param in parameters)
                        {
                            string[] keyValue = param.Split('=');
                            if (keyValue.Length == 2)
                            {
                                result.Add(keyValue[0], keyValue[1]);
                            }
                        }
                    }
                }

                return result;
            }
    }

}
