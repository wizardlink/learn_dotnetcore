var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.MapGet(
    "/",
    async (context) =>
    {
        await context.Response.WriteAsync("Hello!");
    }
);

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();
