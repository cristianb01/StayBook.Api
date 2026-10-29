using Microsoft.EntityFrameworkCore;
using StayBook.Application.Behaviors;
using StayBook.Application.Features.Bookings.Commands;
using StayBook.Application.Mappings;
using StayBook.Extensions;
using StayBook.Hubs;
using StayBook.Infrastructure.Persistence;
using StayBook.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(BookingMappingProfile).Assembly));
builder.Services.AddAuthorization();
builder.Services.ConfigureJwt(builder);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(CreateBookingCommand).Assembly);
    cfg.AddOpenBehavior(typeof(TransactionBehavior<,>));
});

builder.Services.AddApplicationServices();

builder.Services.AddCors(options =>
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    }));

builder.Services.AddMemoryCache();

builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
});

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

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();
app.MapControllers();
app.MapHub<ChatHub>("/hubs/chat");
app.Run();