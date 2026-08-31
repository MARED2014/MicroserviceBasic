using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using PizzaPulse.Ordering.Application.Queries;
using PizzaPulse.Ordering.Core.Repositories;
using PizzaPulse.Ordering.Infrastructure.Contexts;
using PizzaPulse.Ordering.Infrastructure.Repositories;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

builder.Services.AddDbContext<OrderDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSingleton<IConnectionMultiplexer>(sp => ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis")!));
builder.Services.AddScoped<IOrderingRepository, OrderingRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<IMenuRepository, MenuRepository>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetMenuQuery).Assembly));

builder.Services.AddMassTransit(x =>
{
    x.AddConsumers(typeof(GetMenuQuery).Assembly);
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
        Title = "PizzaPulse Ordering API",
        Version = "v1",
        Description = "Menü, sepet ve sipariş API."
    });
});

var app = builder.Build();

try
{
    using var scope = app.Services.CreateScope();
    scope.ServiceProvider.GetRequiredService<OrderDbContext>().Database.EnsureCreated();
}
catch (Exception ex)
{
    app.Logger.LogWarning(ex, "Ordering veritabanı oluşturulamadı.");
}

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Ordering API v1");
    options.DocumentTitle = "PizzaPulse Ordering";
    options.EnableTryItOutByDefault();
    options.DisplayRequestDuration();
});

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();
app.MapControllers();
app.Run();
