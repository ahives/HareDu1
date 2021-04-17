namespace HareDu.Model
{
    using System.Text.Json.Serialization;

    public class VirtualHostDefinition
    {
        public VirtualHostDefinition(bool tracing, string description, string tags)
        {
            Tracing = tracing;
            Description = description;
            Tags = tags;
        }

        public VirtualHostDefinition()
        {
        }

        [JsonPropertyName("tracing")]
        public bool Tracing { get; set; }
        
        [JsonPropertyName("description")]
        public string Description { get; set; }
        
        [JsonPropertyName("tags")]
        public string Tags { get; set; }
    }
}