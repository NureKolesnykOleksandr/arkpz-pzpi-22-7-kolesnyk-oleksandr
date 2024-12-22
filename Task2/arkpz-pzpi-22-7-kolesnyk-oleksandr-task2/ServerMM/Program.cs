using Microsoft.EntityFrameworkCore;
using NETCore.MailKit.Core;
using ServerMM;
using ServerMM.Interfaces;
using ServerMM.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Регистрация сервисов до построения приложения
builder.Services.AddDbContext<SqliteDBContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAlertRepository, AlertRepository>();
builder.Services.AddScoped<IDeviceRepository, DeviceRepository>();
builder.Services.AddScoped<IRecomendationRepository, RecomendationRepository>();
builder.Services.AddScoped<ISensorDataRepository, SensorDataRepository>();

// Добавление контроллеров и других сервисов
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Конфигурация HTTP-конвейера
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
