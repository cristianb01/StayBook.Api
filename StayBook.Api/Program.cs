using Microsoft.EntityFrameworkCore;
using StayBook.Application.Features.Bookings.Commands;
using StayBook.Application.Interfaces;
using StayBook.Application.Mappings;
using StayBook.Extensions;
using StayBook.Infrastructure.Helpers;
using StayBook.Infrastructure.Models;
using StayBook.Infrastructure.Persistence;
using StayBook.Infrastructure.Persistence.Repositories;
using StayBook.Infrastructure.Persistence.UnitOfWork;
using StayBook.Infrastructure.Services;
using StayBook.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(BookingMappingProfile).Assembly));

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateBookingCommand).Assembly));

builder.Services.AddApplicationServices();

builder.Services.AddCors(options =>
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    }));

builder.Services.ConfigureJwt(builder);

builder.Services.AddMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAngularApp");
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();
app.MapControllers();
app.Run();