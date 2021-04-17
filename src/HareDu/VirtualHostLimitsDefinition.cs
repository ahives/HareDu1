namespace HareDu
{
    using System.Text.Json.Serialization;

    public class VirtualHostLimitsDefinition
    {
        public VirtualHostLimitsDefinition(ulong maxConnectionLimit, ulong maxQueueLimit)
        {
            MaxConnectionLimit = maxConnectionLimit;
            MaxQueueLimit = maxQueueLimit;
        }

        public VirtualHostLimitsDefinition()
        {
        }

        [JsonPropertyName("max-connections")]
        public ulong MaxConnectionLimit { get; set; }
        
        [JsonPropertyName("max-queues")]
        public ulong MaxQueueLimit { get; set; }
    }
}