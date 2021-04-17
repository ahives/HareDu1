namespace HareDu.Model
{
    using System.Text.Json.Serialization;

    public class UserPermissionsDefinition
    {
        public UserPermissionsDefinition(string configure, string write, string read)
        {
            Configure = configure;
            Write = write;
            Read = read;
        }

        public UserPermissionsDefinition()
        {
        }

        [JsonPropertyName("configure")]
        public string Configure { get; set; }
        
        [JsonPropertyName("write")]
        public string Write { get; set; }
        
        [JsonPropertyName("read")]
        public string Read { get; set; }
    }
}