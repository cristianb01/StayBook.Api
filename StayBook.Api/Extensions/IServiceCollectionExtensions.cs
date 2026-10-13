using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using StayBook.Application.Interfaces;
using StayBook.Infrastructure.Helpers;
using StayBook.Infrastructure.Models;
using StayBook.Infrastructure.Persistence.Repositories;
using StayBook.Infrastructure.Persistence.UnitOfWork;
using StayBook.Infrastructure.Services;

namespace StayBook.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IBookingRepository, BookingRepository>();
        services.AddScoped<IPropertyRepository, PropertyRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPaymentService, FakePaymentService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IMySqlPropertyLock, MySqlPropertyLock>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddScoped<IConversationRepository, ConversationRepository>();
        
        return services;
    }

    public static IServiceCollection ConfigureJwt(this IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.Audience = services.BuildServiceProvider().GetRequiredService<IConfiguration>()["Jwt:Audience"];
                options.RequireHttpsMetadata = false;
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = ctx =>
                    {
                        var token = ctx.Request.Query["access_token"];
                        if (!string.IsNullOrEmpty(token) && ctx.HttpContext.Request.Path.StartsWithSegments("/hubs/chat"))
                            ctx.Token = token;
                        return Task.CompletedTask;
                    }
                };
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
                };
            });

        builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));
        return services;
    }
}