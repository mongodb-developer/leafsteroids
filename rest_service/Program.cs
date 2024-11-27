using MongoDB.Bson.Serialization;
using RestService.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// required for methods in RecordingsController with diff name but same signature
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "NewPolicy",
        builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
    );
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors("NewPolicy");
app.UseAuthorization();

app.MapControllers();

// Register custom serializers
BsonSerializer.RegisterSerializer(new RecordingPlayerSerializer());
BsonSerializer.RegisterSerializer(new RecordingEventSerializer());
app.Run();

