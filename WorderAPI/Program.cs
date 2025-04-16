using System;
using WorderAPI.Repositories;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(provider =>
{
    try
    {
        return TimeZoneInfo.FindSystemTimeZoneById("South Africa Standard Time"); // Windows
    }
    catch (TimeZoneNotFoundException)
    {
        return TimeZoneInfo.FindSystemTimeZoneById("Africa/Johannesburg"); // Linux/Docker
    }
});

var connectionString = builder.Configuration.GetConnectionString("WORD_DB");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<IWordRepositoryAsync, WordRepositoryAsync>();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
