using Models;

namespace Contracts;

public interface IFinnhubService
{
    public Task<CompanyProfile> GetCompanyProfile(string symbol);
    public Task<StockPriceQuote> GetStockPriceQuote(string symbol);
}
