using Booking.Application;
using Booking.Infrastructure;
using Booking.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using UnbeatableBookingSystem.Controllers.Base;
using UnbeatableBookingSystem.Middlewares;
using UnbeatableBookingSystem.Utility;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(AuthorizationConfiguration.ConfigureSwaggerWithJwtBearer);

builder.Services.AddControllers();

builder.Services.AddHostedService<RevokedAccessTokenCleanupService>();
builder.Services.AddApplication();
builder.Services.AddInfrastructure();
builder.Services.AddScoped<ControllerUtils>();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(AuthorizationConfiguration.ConfigureJwtBearerAuthorization);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactHttpOrigin",
        policy => policy.WithOrigins("http://localhost:5175", "http://localhost:8080")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<RevokedAccessTokenMiddleware>();

app.UseCors("AllowReactHttpOrigin");
app.UseHttpsRedirection();
app.MapControllers();
app.UseStaticFiles();

using (var scope = app.Services.CreateScope())
{
    InfrastructureStartup.CheckAndMigrateDatabase(scope);
}

app.Run();