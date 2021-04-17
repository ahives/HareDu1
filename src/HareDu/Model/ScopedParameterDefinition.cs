namespace HareDu.Model
{
    using System.Text.Json.Serialization;

    public class ScopedParameterDefinition<T>
    {
        public ScopedParameterDefinition(string virtualHost, string component, string parameterName, T parameterValue)
        {
            VirtualHost = virtualHost;
            Component = component;
            ParameterName = parameterName;
            ParameterValue = parameterValue;
        }

        public ScopedParameterDefinition()
        {
        }

        [JsonPropertyName("vhost")]
        public string VirtualHost { get; set; }

        [JsonPropertyName("component")]
        public string Component { get; set; }

        [JsonPropertyName("name")]
        public string ParameterName { get; set; }

        [JsonPropertyName("value")]
        public T ParameterValue { get; set; }
    }
}