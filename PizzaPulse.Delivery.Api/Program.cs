using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using PizzaPulse.Delivery.Application.Consumers;
using PizzaPulse.Delivery.Application.Queries;
using PizzaPulse.Delivery.Core.Repositories;
using PizzaPulse.Delivery.Infrastructure.Contexts;
using PizzaPulse.Delivery.Infrastructure.Repositories;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

builder.Services.AddDbContext<DeliveryDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSingleton<IConnectionMultiplexer>(sp => ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis")!));
builder.Services.AddScoped<IDeliveryRepository, DeliveryRepository>();
builder.Services.AddScoped<ICourierRepository, CourierRepository>();
builder.Services.AddScoped<ICourierStateRepository, CourierStateRepository>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetDeliveryAssignmentQuery).Assembly));

builder.Services.AddMassTransit(x =>
{
    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("delivery", false));
    x.AddConsumers(typeof(OrderBakedConsumer).Assembly);
    x.UsingRabbitMq((context, cfg) =>
    {
        var rabbit = builder.Configuration.GetSection("RabbitMQ");
        cfg.Host(rabbit["Host"] ?? "localhost", ushort.Parse(rabbit["Port"] ?? "5672"), "/", h =>
        {
            h.Username(rabbit["Username"] ?? "admin");
            h.Password(rabbit["Password"] ?? "Password123!");
        });
        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "PizzaPulse Delivery API",
        Version = "v1",
        Description = "Kurye atama ve teslimat API."
    });
});

var app = builder.Build();

try
{
    using var scope = app.Services.CreateScope();
    scope.ServiceProvider.GetRequiredService<DeliveryDbContext>().Database.EnsureCreated();
}
catch (Exception ex)
{
    app.Logger.LogWarning(ex, "Delivery veritabanı oluşturulamadı.");
}

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Delivery API v1");
    options.DocumentTitle = "PizzaPulse Delivery";
    options.EnableTryItOutByDefault();
    options.DisplayRequestDuration();
});

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();
app.MapControllers();
app.Run();
