using TestApi.CustomConstraints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting(options =>
{
    options.ConstraintMap.Add("months", typeof(MonthsCustomConstraint));
});

var app = builder.Build();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.Map(
        "files/{filename}.{extension}",
        async context =>
        {
            string? fileName = Convert.ToString(context.Request.RouteValues["filename"]);
            string? extension = Convert.ToString(context.Request.RouteValues["extension"]);
            if (fileName == null || extension == null)
            {
                return;
            }

            await context.Response.WriteAsync($"In files - {fileName}.{extension}");
        }
    );

    endpoints.Map(
        "employee/profile/{EmployeeName:alpha:length(3,7)=harsha}",
        async context =>
        {
            string? employeeName = Convert.ToString(context.Request.RouteValues["EmployeeName"]);
            await context.Response.WriteAsync($"In employee - {employeeName}");
        }
    );

    endpoints.Map(
        "products/details/{id:int:min(1):max(1000)?}",
        async context =>
        {
            if (context.Request.RouteValues.ContainsKey("id"))
            {
                int? id = Convert.ToInt32(context.Request.RouteValues["id"]);
                await context.Response.WriteAsync($"Products details - {id}");
            }
            else
            {
                await context.Response.WriteAsync("ID is not supplied.");
            }
        }
    );

    endpoints.Map(
        "daily-digest-report/{reportdate:datetime}",
        async context =>
        {
            if (context.Request.RouteValues.ContainsKey("reportdate"))
            {
                DateTime? dateTime = Convert.ToDateTime(context.Request.RouteValues["datetime"]);
                await context.Response.WriteAsync($"Products details - {dateTime.Value.ToLongDateString()}");
            }
            else
            {
                await context.Response.WriteAsync("Date is not supplied.");
            }
        }
    );

    endpoints.Map(
        "cities/{cityid:guid}",
        async context =>
        {
            if (context.Request.RouteValues.ContainsKey("cityid"))
            {
                Guid? cityId = Guid.Parse(Convert.ToString(context.Request.RouteValues["cityid"])!);
                await context.Response.WriteAsync($"Products details - {cityId}");
            }
            else
            {
                await context.Response.WriteAsync("Date is not supplied.");
            }
        }
    );

    endpoints.Map(
        "sales-report/{year:int:min(1900)}/{month:months}",
        async context =>
        {
            int year = Convert.ToInt32(context.Request.RouteValues["year"]);
            string? month = Convert.ToString(context.Request.RouteValues["month"]);

            await context.Response.WriteAsync($"sales report - {year} - {month}");
        }
    );
});

app.Run(async context =>
{
    await context.Response.WriteAsync($"No route matched at {context.Request.Path}");
});

app.Run();
