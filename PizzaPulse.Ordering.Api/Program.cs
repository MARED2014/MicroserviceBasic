using Microsoft.EntityFrameworkCore;
using PizzaPulse.Ordering.Core.Repositories;
using PizzaPulse.Ordering.Infrastructure.Contexts;
using PizzaPulse.Ordering.Infrastructure.Repositories;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);


// 1. MS SQL & DbContext Kaydı
builder.Services.AddDbContext<OrderDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Redis Connection Multiplexer Kaydı (Singleton)
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
    ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis")!));

// 3. Repository Kayıtları (Scoped)
builder.Services.AddScoped<IOrderingRepository, OrderingRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();


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
