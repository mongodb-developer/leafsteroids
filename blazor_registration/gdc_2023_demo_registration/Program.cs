using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using gdc_2023_demo_registration.Data;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

static void ConfigureServices(IServiceCollection services)
{
    IConfiguration config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .Build();

    services.AddSingleton<IMongoClient>(new MongoClient(config.GetConnectionString("MongoDB")));
    services.AddSingleton<IMongoDatabase>(x => x.GetRequiredService<IMongoClient>().GetDatabase("registration"));
    services.AddSingleton<IMongoCollection<Team>>(x => x.GetRequiredService<IMongoDatabase>().GetCollection<Team>("players"));
}

ConfigureServices(builder.Services);

    
// builder.Services.AddSingleton<IMongoClient>(new MongoClient(Configuration.GetConnectionString("MongoDB")));
// builder.Services.AddSingleton<IMongoDatabase>(x => x.GetRequiredService<IMongoClient>().GetDatabase("yourDatabaseName"));
// builder.Services.AddSingleton<IMongoCollection<Team>>(x => x.GetRequiredService<IMongoDatabase>().GetCollection<Team>("yourCollectionName"));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();