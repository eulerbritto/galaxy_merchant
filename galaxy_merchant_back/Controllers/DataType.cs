using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace galaxy_merchant_back.Controllers.DataType
{
    public enum RomanVal
    {
        I = 1,
        V = 5,
        X = 10,
        L = 50,
        C = 100,
        D = 500,
        M = 1000
    }
    public enum PhraseType
    {
        Set,
        Compute,
        Query
    }    
    public class DB 
    {
        public DB (){
            galaxylang  = new Dictionary<string, RomanVal>();            
            words_values = new Dictionary<string, float>();
            coin = new List<string> { "GLOB", "PROK", "PISH", "TEGJ" };
        }
        public Dictionary<string, RomanVal> galaxylang;        
        public Dictionary<string, float> words_values;
        public List<string> coin;
    }
    public static class SessionExtensions
    {
        public static void SetObjectAsJson<T>(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }
    
        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);                
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
}