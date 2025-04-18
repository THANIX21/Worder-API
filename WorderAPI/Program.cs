using WorderAPI.Repositories;
using WorderAPI.Repositories.Base;
using WorderAPI.Repositories.Interfaces;

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
builder.Services.AddCors();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<IWordRepositoryAsync, WordRepositoryAsync>();
builder.Services.AddScoped<ISentenceRepositoryAsync, SentenceRepositoryAsync>();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors(options => options.AllowAnyOrigin()
                              .AllowAnyHeader()
                              .AllowAnyMethod());

app.Run();
