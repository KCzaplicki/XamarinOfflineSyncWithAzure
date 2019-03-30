using System;
using Newtonsoft.Json;

namespace XamarinOfflineSyncWithAzure.Models
{
    public class Project
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime UpdatedAt { get; set; }

        [JsonProperty(PropertyName = "version")]
        public string Version { set; get; }
    }
}
