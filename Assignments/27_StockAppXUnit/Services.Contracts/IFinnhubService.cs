using Models.DTO.Finnhub;

namespace Services.Contracts;

public interface IFinnhubService
{
    public Task<CompanyProfile?> GetCompanyProfile(string stockSymbol);

    public Task<StockPriceQuote?> GetStockPriceQuote(string stockSymbol);
}
