using gdc_2023_demo_registration.Data;
using MongoDB.Driver;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

static void ConfigureServices(IServiceCollection services)
{
    var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
    var mongoClient = new MongoClient(connectionString!);
    services.AddSingleton<IMongoClient>(mongoClient);
    services.AddSingleton<IMongoDatabase>(x => x.GetRequiredService<IMongoClient>().GetDatabase("Leafsteroids")!);
    services.AddSingleton<IMongoCollection<Player>>(x =>
        x.GetRequiredService<IMongoDatabase>().GetCollection<Player>("players")!);
    services.AddSingleton<IMongoCollection<PlayerUnique>>(x =>
        x.GetRequiredService<IMongoDatabase>().GetCollection<PlayerUnique>("players_unique")!);
    services.AddSingleton<IMongoCollection<Event>>(x =>
        x.GetRequiredService<IMongoDatabase>().GetCollection<Event>("events")!);
    services.AddSingleton<IMongoCollection<Recording>>(x =>
        x.GetRequiredService<IMongoDatabase>().GetCollection<Recording>("recordings")!);
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