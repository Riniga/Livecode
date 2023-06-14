using Newtonsoft.Json;

namespace Livecode.Library
{
    public class User
    {
        [JsonProperty(PropertyName = "id")]
        public string Id => Fingerprint;
        [JsonProperty(PropertyName = "fingerprint")]
        public string Fingerprint { get; set; }
        [JsonProperty(PropertyName = "timestamp")]
        public DateTime Timestamp { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "length")]
        public int Length { get; set; }
        [JsonProperty(PropertyName = "weight")]
        public int Weight { get; set; }
        [JsonProperty(PropertyName = "browser")]
        public string Browser { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        private User()
        {
            Fingerprint = "";Name = "";Timestamp = DateTime.Now;Fingerprint = "";Browser = "";
        }
    }
}
