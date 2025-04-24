using Fizzler.Systems.HtmlAgilityPack;
using FluentAssertions;
using HtmlAgilityPack;

namespace Services.Test.Integrations;

public class Explore(IntegrationClientFactory factory) : BaseClass(factory)
{
    [Fact]
    public async Task IndexHasStockList()
    {
        var response = await _client.GetAsync("/explore");

        response.Should().Be2XXSuccessful();

        var body = await response.Content.ReadAsStringAsync();

        var htmlDocument = new HtmlDocument();

        htmlDocument.LoadHtml(body);

        var document = htmlDocument.DocumentNode;

        document.QuerySelector("div.stock-list").Should().NotBeNull();
    }

    [Fact]
    public async Task IndexHasCompanyInfo()
    {
        var response = await _client.GetAsync("/explore/GOOG");

        response.Should().Be2XXSuccessful();

        var body = await response.Content.ReadAsStringAsync();

        var htmlDocument = new HtmlDocument();

        htmlDocument.LoadHtml(body);

        var document = htmlDocument.DocumentNode;

        document.QuerySelector("div.stock-list").Should().NotBeNull();

        document.QuerySelector("div.company-info").Should().NotBeNull();
    }
}
