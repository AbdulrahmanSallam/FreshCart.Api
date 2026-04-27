using FreshCart.Infrastructure;
using FreshCart.Infrastructure.Common.Persistence;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddOpenApi();

    builder.Services.AddInfrastructure(builder.Configuration);
}

var app = builder.Build();
{
    await app.Services.InitializeDatabaseAsync();

    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
    }

    app.UseHttpsRedirection();
}

app.Run();