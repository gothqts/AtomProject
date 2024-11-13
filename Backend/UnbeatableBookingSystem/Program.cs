using Booking.Application;
using Booking.Infrastructure;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddApplication();
builder.Services.AddInfrastructure();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/api/auth/login";  // API для логина
        options.LogoutPath = "/api/auth/logout"; // API для выхода
        options.ExpireTimeSpan = TimeSpan.FromDays(7); // Время действия куки
        options.SlidingExpiration = true; // Обновление срока действия при активности
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.UseStaticFiles();

using (var scope = app.Services.CreateScope())
{
    try
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<BookingDbContext>();
        dbContext.Database.Migrate();
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        throw;
    }
}

app.Run();