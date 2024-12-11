using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using Back_end.Data;
using Back_end.ExtensionHub;
using Back_end.Service;
using Back_end.Repository;
using Back_end.Mapper;
using Microsoft.Extensions.Caching.Distributed;  // Đảm bảo có namespace này cho DistributedCache
//using Microsoft.Extensions.Caching.StackExchangeRedis;  // Dành cho Redis, nếu bạn sử dụng Redis

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
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Cấu hình Redis cho Distributed Cache (session phân tán)
builder.Services.AddDistributedMemoryCache();

// Cấu hình session sử dụng DistributedCache (Redis)
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = false;
    options.Cookie.IsEssential = false;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    options.Cookie.SameSite = SameSiteMode.None;
});

// Cấu hình DbContext
builder.Services.AddDbContext<CoreContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CoreContext")));

// Thêm dịch vụ cho SignalR và các dịch vụ khác
builder.Services.AddScoped<IAdafruidService, AdafruidService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserMapper, UserMapper>();

// Thêm SignalR
builder.Services.AddSignalR();

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

// Áp dụng session middleware
app.UseSession();

app.UseAuthorization();

app.MapControllers();

// Thêm SignalR Hub vào pipeline
app.MapHub<TemperatureHub>("/temperatureHub");

app.Run();
