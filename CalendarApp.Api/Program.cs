using FastEndpoints.Swagger;
using FastEndpoints;
using CalendarApp.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using CalendarApp.DataAccess.Data;
using CalendarApp.Core.Interfaces;
using CalendarApp.Application.Validators;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=calendar-app.db";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddScoped<ICalendarAppRepository, CalendarEventRepository>();
builder.Services.AddScoped<EventModelValidator>();
builder.Services.AddScoped<EventIdValidator>();

builder.Services.AddFastEndpoints();
builder.Services.SwaggerDocument();
builder.Services.AddAuthorization();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(builder.Configuration["FrontendUrl"] ?? "http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors("AllowFrontend");

app.UseRouting();
app.UseAuthorization();
app.UseFastEndpoints();

app.UseOpenApi();
app.UseSwaggerUI();

app.Run();