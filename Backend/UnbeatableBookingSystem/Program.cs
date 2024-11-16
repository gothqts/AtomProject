using Booking.Application;
using Booking.Infrastructure;
using Booking.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using UnbeatableBookingSystem.Middlewares;
using UnbeatableBookingSystem.Utility;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(AuthorizationConfiguration.ConfigureSwaggerWithJwtBearer);

builder.Services.AddControllers();

builder.Services.AddHostedService<RevokedAccessTokenCleanupService>();
builder.Services.AddApplication();
builder.Services.AddInfrastructure();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(AuthorizationConfiguration.ConfigureJwtBearerAuthorization);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<RevokedAccessTokenMiddleware>();

app.UseHttpsRedirection();
app.MapControllers();
app.UseStaticFiles();

using (var scope = app.Services.CreateScope())
{
    InfrastructureStartup.CheckAndMigrateDatabase(scope);
}

app.Run();