using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using static SimpleWebSocketServer.SIBS.Lib.Enums.Enums;

namespace SimpleWebSocketServer.SIBS.Lib.Models
{
    public class ReceiptNotification
    {
        [JsonProperty("receipt")]
        public Receipt Receipt { get; set; }

        [JsonProperty("receiptType")]
        public string ReceiptType { get; set; }

        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public RequestType Type { get; set; } = RequestType.TX_RESPONSE;

        [JsonProperty("version")]
        public string Version { get; set; }
    }
    public class Block
    {
        [JsonProperty("attributes")]
        public int Attributes { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }

    public class Footer
    {
        [JsonProperty("lines")]
        public List<Line> Lines { get; set; }
    }

    public class Header
    {
        [JsonProperty("lines")]
        public List<Line> Lines { get; set; }
    }

    public class Line
    {
        [JsonProperty("blocks")]
        public List<Block> Blocks { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("graphic")]
        public string Graphic { get; set; }
    }

    public class Receipt
    {
        [JsonProperty("footer")]
        public Footer Footer { get; set; }

        [JsonProperty("format")]
        public string Format { get; set; }

        [JsonProperty("header")]
        public Header Header { get; set; }
    }
}
