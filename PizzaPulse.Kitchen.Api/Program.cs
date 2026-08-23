using MongoDB.Driver;
using PizzaPulse.Kitchen.Core.Repositories;
using PizzaPulse.Kitchen.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// 1. MongoDB Client ve Database Kaydı (Singleton)
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var connectionString = builder.Configuration["MongoDbSettings:ConnectionString"];
    return new MongoClient(connectionString);
});

builder.Services.AddScoped<IMongoDatabase>(sp =>
{
    var client = sp.GetRequiredService<IMongoClient>();
    var dbName = builder.Configuration["MongoDbSettings:DatabaseName"];
    return client.GetDatabase(dbName);
});

// 2. Generic Mongo Repository Kaydı (Open Generic Registration)
builder.Services.AddScoped(typeof(IMongoRepository<>), typeof(MongoRepository<>));

// 3. Specific Kitchen Repository Kaydı
builder.Services.AddScoped<IKitchenTaskRepository, KitchenTaskRepository>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
