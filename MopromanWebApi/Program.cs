using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MopromanWebApi.Models;
using PeriodicBackgroundTaskSample;

string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddScoped<SampleService>();

// Register as singleton first so it can be injected through Dependency Injection
builder.Services.AddSingleton<PeriodicHostedService>();

// Add as hosted service using the instance registered as singleton before
builder.Services.AddHostedService(
    provider => provider.GetRequiredService<PeriodicHostedService>());


// Add services to the container.
builder.Services.AddControllers();
var connectionString = builder.Configuration.GetConnectionString("MopromanCS");
builder.Services.AddDbContext<MopromanDbContext>(options =>
    options.UseSqlServer(connectionString));


builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          /*
                          policy.WithOrigins("http://example.com",
                                              "http://www.contoso.com",
                                              "http://localhost:5173",
                                              "https://localhost:5173"
                                              );*/
                          policy.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
                      });
});


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.MapGet("/background", (
    PeriodicHostedService service) =>
{
    return new PeriodicHostedServiceState(service.IsEnabled);
});
/*
app.MapMethods("/background", new[] { "PATCH" }, (
    PeriodicHostedServiceState state,
    PeriodicHostedService service) =>
{
    service.IsEnabled = state.IsEnabled;
});*/

app.Run();
