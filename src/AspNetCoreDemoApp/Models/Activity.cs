using Newtonsoft.Json;
using System;

namespace AspNetCoreDemoApp.Models
{
    public class Activity
    {
        [JsonProperty("events")]
        public Event[] Events { get; set; }        
    }
}
