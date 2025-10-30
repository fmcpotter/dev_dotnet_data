using System;
using Data.Context;
using Data.Repositories;
using Domain.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

// Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

// Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext — InMemory for prototype
builder.Services.AddDbContext<SalesContext>(opt => opt.UseInMemoryDatabase("SalesDb"));

// Repositories
builder.Services.AddScoped<ISaleRepository, SaleRepository>();

// Simple Event Publisher (logs events)
builder.Services.AddSingleton<Domain.Services.IEventPublisher, Domain.Services.LoggingEventPublisher>();

var app = builder.Build();

// Seed sample data
using (var scope = app.Services.CreateScope())
{
    var ctx = scope.ServiceProvider.GetRequiredService<SalesContext>();
    ctx.Database.EnsureCreated();
}

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();