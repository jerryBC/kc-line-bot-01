using Newtonsoft.Json;

namespace AspNetCoreDemoApp.Models
{
    public abstract class Template
    {
        [JsonProperty("type")]
        public TemplateType Type { get; internal set; }
    }
}
