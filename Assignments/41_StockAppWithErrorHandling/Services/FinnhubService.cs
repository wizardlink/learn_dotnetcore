using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Models.DTO.Finnhub;
using Serilog;
using SerilogTimings;
using Services.Contracts;

namespace Services;

public class FinnhubService(IHttpClientFactory clientFactory, IConfiguration configuration, ILogger logger)
    : IFinnhubService
{
    private static readonly JsonSerializerOptions insensitiveDeserializeOptions = new()
    {
        PropertyNameCaseInsensitive = true,
    };

    private string GetToken()
    {
        string? token = configuration["FinnhubToken"];

        if (token == null || token == string.Empty)
        {
            throw new MissingFieldException("The FinnhubToken configuration field is either null or empty.");
        }

        return token;
    }

    public async Task<CompanyProfile?> GetCompanyProfile(string stockSymbol)
    {
        using HttpClient client = clientFactory.CreateClient();

        HttpResponseMessage response = await client.SendAsync(
            new HttpRequestMessage
            {
                RequestUri = new Uri(
                    $"https://finnhub.io/api/v1/stock/profile2?symbol={stockSymbol}&token={GetToken()}"
                ),
                Method = HttpMethod.Get,
            }
        );

        var text = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<CompanyProfile>(
            await response.Content.ReadAsStreamAsync(),
            insensitiveDeserializeOptions
        );
    }

    public async Task<StockPriceQuote?> GetStockPriceQuote(string stockSymbol)
    {
        using HttpClient client = clientFactory.CreateClient();

        HttpResponseMessage response = await client.SendAsync(
            new HttpRequestMessage
            {
                RequestUri = new Uri($"https://finnhub.io/api/v1/quote?symbol={stockSymbol}&token={GetToken()}"),
                Method = HttpMethod.Get,
            }
        );

        return JsonSerializer.Deserialize<StockPriceQuote>(await response.Content.ReadAsStreamAsync());
    }

    public async Task<List<Stock>?> GetStocks()
    {
        logger.Information("Entered FinnhubService's GetStocks");

        using HttpClient client = clientFactory.CreateClient();

        HttpResponseMessage? response;

        using (Operation.Time("GetStocks' Time"))
            response = await client.SendAsync(
                new HttpRequestMessage
                {
                    RequestUri = new Uri($"https://finnhub.io/api/v1/stock/symbol?exchange=US&token={GetToken()}"),
                    Method = HttpMethod.Get,
                }
            );

        return JsonSerializer.Deserialize<List<Stock>>(
            await response.Content.ReadAsStreamAsync(),
            insensitiveDeserializeOptions
        );
    }
}
