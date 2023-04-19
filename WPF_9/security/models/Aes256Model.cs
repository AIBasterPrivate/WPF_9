using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_9.security.models
{
    public class Aes256Model
    {
        [JsonProperty("key")]
        public byte[] Key { get; private set; } 

        [JsonProperty("iv")]
        public byte[] IV { get; private set; } 

        [NonSerialized]
        public Encoding Encoding;

        public static Aes256Model Load(string configFilePath)
        {
            using (StreamReader reader = new StreamReader(configFilePath))
            {
                string json = reader.ReadToEnd();
                Aes256Model aes = JsonConvert.DeserializeObject<Aes256Model>(json);
                if(aes == null)
                {
                    throw new Exception("appconfig is empty!");
                }
                return aes;
            }
        }
    }
}
