using System.Text.Json;
using Contracts;
using Microsoft.Extensions.Configuration;
using Models;

namespace Services;

public class FinnhubService : IFinnhubService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly IConfiguration _configuration;

    public FinnhubService(IHttpClientFactory clientFactory, IConfiguration configuration)
    {
        _clientFactory = clientFactory;
        _configuration = configuration;
    }

    private string GetToken()
    {
        string? token = _configuration["FinnhubToken"];

        if (token == null || token == string.Empty)
        {
            throw new MissingFieldException("The FinnhubToken configuration field is either null or empty.");
        }

        return token;
    }

    public async Task<CompanyProfile> GetCompanyProfile(string symbol)
    {
        using (HttpClient client = _clientFactory.CreateClient())
        {
            HttpResponseMessage response = await client.SendAsync(
                new HttpRequestMessage
                {
                    RequestUri = new Uri(
                        $"https://finnhub.io/api/v1/stock/profile2?symbol={symbol}&token={GetToken()}"
                    ),
                    Method = HttpMethod.Get,
                }
            );

            return JsonSerializer.Deserialize<CompanyProfile>(await response.Content.ReadAsStreamAsync())
                ?? new CompanyProfile();
        }
    }

    public async Task<StockPriceQuote> GetStockPriceQuote(string symbol)
    {
        using (HttpClient client = _clientFactory.CreateClient())
        {
            HttpResponseMessage response = await client.SendAsync(
                new HttpRequestMessage
                {
                    RequestUri = new Uri($"https://finnhub.io/api/v1/quote?symbol={symbol}&token={GetToken()}"),
                    Method = HttpMethod.Get,
                }
            );

            return JsonSerializer.Deserialize<StockPriceQuote>(await response.Content.ReadAsStreamAsync())
                ?? new StockPriceQuote();
        }
    }
}
