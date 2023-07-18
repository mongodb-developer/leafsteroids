using dotenv.net;
using MongoDB.Driver;
using website.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

DotEnv.Load();

static void ConfigureServices(IServiceCollection services)
{
    var envVars = DotEnv.Read();
    var connectionString = envVars["CONNECTION_STRING"];
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