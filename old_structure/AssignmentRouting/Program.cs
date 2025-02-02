var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

Dictionary<int, string> countries = new Dictionary<int, string>
{
    { 1, "United States" },
    { 2, "Canada" },
    { 3, "United Kingdom" },
    { 4, "India" },
    { 5, "Japan" },
};

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGet(
        "/countries",
        async httpContext =>
        {
            string countriesText = string.Empty;

            foreach (KeyValuePair<int, string> item in countries)
            {
                countriesText += $"{item.Key}, {item.Value}\n";
            }

            await httpContext.Response.WriteAsync(countriesText);
        }
    );

    endpoints.MapGet(
        "/countries/{code:int:range(1, 100)}",
        async httpContext =>
        {
            if (!httpContext.Request.RouteValues.ContainsKey("code"))
            {
                httpContext.Response.StatusCode = 500;
                return;
            }

            int code = Convert.ToInt32(httpContext.Request.RouteValues["code"]);

            if (code > countries.Count)
            {
                httpContext.Response.StatusCode = 404;
                await httpContext.Response.WriteAsync("[No country]");
                return;
            }

            await httpContext.Response.WriteAsync($"{countries[code]}");
        }
    );

    endpoints.MapGet(
        "/countries/{code:int:min(101)}",
        async httpContext =>
        {
            httpContext.Response.StatusCode = 400;
            await httpContext.Response.WriteAsync("The CountryID should be between 1 and 100");
        }
    );
});

app.Run();
