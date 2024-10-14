using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using Back_end.Data;
using Back_end.ExtensionHub;
using Back_end.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Cấu hình CORS để cho phép Angular kết nối
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200")  // URL của ứng dụng Angular
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();                    // Cho phép sử dụng cookies nếu cần
    });
});

// Cấu hình DbContext
builder.Services.AddDbContext<CoreContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CoreContext")));

// Thêm dịch vụ cho SignalR và các dịch vụ khác
builder.Services.AddSingleton<IAdafruidService, AdafruidService>();
builder.Services.AddSignalR();  // Thêm SignalR

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Áp dụng CORS cho các request
app.UseCors("AllowAngularApp");

app.UseAuthorization();

app.MapControllers();

// Thêm SignalR Hub vào pipeline
app.MapHub<TemperatureHub>("/temperatureHub");  // Đường dẫn cho SignalR Hub

app.Run();
