namespace HareDu.Model
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public class ExchangeDefinition
    {
        public ExchangeDefinition(ExchangeRoutingType? routingType, bool durable, bool autoDelete, bool @internal, IDictionary<string, object> arguments)
        {
            if (routingType.HasValue)
                RoutingType = routingType.Value;
            
            Durable = durable;
            AutoDelete = autoDelete;
            Internal = @internal;
            Arguments = arguments;
        }

        public ExchangeDefinition()
        {
        }

        [JsonPropertyName("type")]
        public ExchangeRoutingType RoutingType { get; set; }
        
        [JsonPropertyName("durable")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool Durable { get; set; }
        
        [JsonPropertyName("auto_delete")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool AutoDelete { get; set; }
        
        [JsonPropertyName("internal")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool Internal { get; set; }
        
        [JsonPropertyName("arguments")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public IDictionary<string, object> Arguments { get; set; }
    }
}