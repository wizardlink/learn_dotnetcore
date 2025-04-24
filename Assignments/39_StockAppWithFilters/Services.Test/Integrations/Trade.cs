using Fizzler.Systems.HtmlAgilityPack;
using FluentAssertions;
using HtmlAgilityPack;

namespace Services.Test.Integrations;

public class Trade(IntegrationClientFactory factory) : BaseClass(factory)
{
    [Fact]
    public async Task IndexHasTradeButtons()
    {
        var response = await _client.GetAsync("/");

        response.Should().Be2XXSuccessful();

        var body = await response.Content.ReadAsStringAsync();

        var htmlDocument = new HtmlDocument();

        htmlDocument.LoadHtml(body);

        var document = htmlDocument.DocumentNode;

        document.QuerySelector("button.sell-button").Should().NotBeNull();
        document.QuerySelector("button.buy-button").Should().NotBeNull();
    }

    [Fact]
    public async Task OrdersHasList()
    {
        var response = await _client.GetAsync("/orders");

        response.Should().Be2XXSuccessful();

        var body = await response.Content.ReadAsStringAsync();

        var htmlDocument = new HtmlDocument();

        htmlDocument.LoadHtml(body);

        var document = htmlDocument.DocumentNode;

        document.QuerySelector("div.order-list").Should().NotBeNull();
    }
}
