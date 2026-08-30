using MassTransit;
using Microsoft.OpenApi;
using MongoDB.Driver;
using PizzaPulse.Kitchen.Application.Queries;
using PizzaPulse.Kitchen.Core.Repositories;
using PizzaPulse.Kitchen.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

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

builder.Services.AddScoped(typeof(IMongoRepository<>), typeof(MongoRepository<>));
builder.Services.AddScoped<IKitchenTaskRepository, KitchenTaskRepository>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetKitchenTaskQuery).Assembly));

builder.Services.AddMassTransit(x =>
{
    x.AddConsumers(typeof(GetKitchenTaskQuery).Assembly);
    x.UsingRabbitMq((context, cfg) =>
    {
        var rabbit = builder.Configuration.GetSection("RabbitMQ");
        cfg.Host(rabbit["Host"] ?? "localhost", ushort.Parse(rabbit["Port"] ?? "5672"), "/", h =>
        {
            h.Username(rabbit["Username"] ?? "guest");
            h.Password(rabbit["Password"] ?? "guest");
        });
        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "PizzaPulse Kitchen API",
        Version = "v1",
        Description = "Mutfak kuyruğu API."
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Kitchen API v1");
    options.DocumentTitle = "PizzaPulse Kitchen";
    options.EnableTryItOutByDefault();
    options.DisplayRequestDuration();
});

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();
app.MapControllers();
app.Run();
