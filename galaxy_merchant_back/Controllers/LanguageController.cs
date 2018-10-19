using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using System.Text.RegularExpressions;
using galaxy_merchant_back.Controllers;
using galaxy_merchant_back.Controllers.DataType;
using Microsoft.Extensions.Caching.Memory;

namespace galaxy_merchant_back.Controllers
{
    [Route("api/[controller]")]
    public class LanguageController : Controller
    {              
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        // GET api/values/5 
        [HttpGet("{id}")]
        public string Get(string id)
            {
                return  Newtonsoft.Json.JsonConvert.SerializeObject("value");
            }

        public class Incomming{
            public string Text{get;set;}
        }
        // POST api/values
        [HttpPost]
        public string Post([FromBody]Incomming value)
        { 
            try
            {
                string GalaxyString = value.Text;
                DB lastdata = new DB();
                if(HttpContext.Session.GetObjectFromJson<DB>("data") == null)
                    HttpContext.Session.SetObjectAsJson<DB>("data", lastdata);
                else
                    lastdata = HttpContext.Session.GetObjectFromJson<DB>("data");
                var result = default(string);
                switch (CoinEngine.Phrase(GalaxyString, lastdata))
                {
                    case PhraseType.Set:
                        CoinEngine.TranslateValues(GalaxyString, lastdata);                        
                        break;                       
                    case PhraseType.Compute:                        
                        CoinEngine.ComputeValues(GalaxyString, lastdata);                        
                        break;
                    case PhraseType.Query:                        
                        result = Newtonsoft.Json.JsonConvert.SerializeObject(CoinEngine.AnswerQuestion(GalaxyString, lastdata));
                        HttpContext.Session.SetObjectAsJson<DB>("data", lastdata);
                        return result;
                }                
                HttpContext.Session.SetObjectAsJson<DB>("data", lastdata);                        
                return Newtonsoft.Json.JsonConvert.SerializeObject(string.Empty);       
            }
            catch (UnrecognizeWordException e)
            {                    
                return e.Message;
            }                               
        }
    }
}
