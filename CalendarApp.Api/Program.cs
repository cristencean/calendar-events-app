using FastEndpoints.Swagger;
using FastEndpoints;
using CalendarApp.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using CalendarApp.DataAccess.Data;
using CalendarApp.Core.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=calendar-app.db"));
builder.Services.AddScoped<ICalendarAppRepository, CalendarEventRepository>();

builder.Services.AddFastEndpoints();
builder.Services.SwaggerDocument();
builder.Services.AddAuthorization();

// allow CORS for testing purpose only
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors("AllowFrontend");

app.UseAuthorization();
app.UseFastEndpoints();
app.UseOpenApi();
app.UseSwaggerUI();

app.Run();
