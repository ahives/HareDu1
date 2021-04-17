namespace HareDu
{
    using System.Text.Json.Serialization;

    public class TopicPermissionsDefinition
    {
        public TopicPermissionsDefinition(string exchange, string write, string read)
        {
            Exchange = exchange;
            Write = write;
            Read = read;
        }

        public TopicPermissionsDefinition()
        {
        }

        [JsonPropertyName("exchange")]
        public string Exchange { get; set; }
        
        [JsonPropertyName("write")]
        public string Write { get; set; }
        
        [JsonPropertyName("read")]
        public string Read { get; set; }
    }
}