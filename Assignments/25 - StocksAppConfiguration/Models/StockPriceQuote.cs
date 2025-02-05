using System.Text.Json.Serialization;

namespace Models;

public class StockPriceQuote
{
    [JsonPropertyName("c")]
    public float? CurrentPrice { get; set; }

    [JsonPropertyName("d")]
    public double? Change { get; set; }

    [JsonPropertyName("dp")]
    public double? PercentChange { get; set; }

    [JsonPropertyName("h")]
    public float? HighPrice { get; set; }

    [JsonPropertyName("l")]
    public float? LowPrice { get; set; }

    [JsonPropertyName("o")]
    public float? OpenPrice { get; set; }

    [JsonPropertyName("pc")]
    public float? PreviousClosePrice { get; set; }
}
