﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using static SimpleWebSocketServer.SIBS.Enums.Enums;

namespace SimpleWebSocketServer.SIBS.Models
{
    public class EventNotification
    {
        [JsonProperty("version")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Version Version { get; set; } = Version.V_1;

        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public RequestType Type { get; set; } = RequestType.EVENT_NOTIFICATION;

        [JsonProperty("eventType")]
        public string EventType { get; set; }
    }
}
