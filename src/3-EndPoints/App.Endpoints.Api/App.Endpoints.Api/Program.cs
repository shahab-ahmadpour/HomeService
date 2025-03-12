using App.Domain.Core.Services.Interfaces.IAppService;
using App.Domain.Core.Services.Interfaces.IRepository;
using App.Domain.Core.Services.Interfaces.IService;
using App.Infrastructure.DbAccess.Repository.Dapper.Services;
using HomeService.Domain.AppServices.CategoryAppServices;
using HomeService.Domain.AppServices.HomeServiceAppServices;
using HomeService.Domain.AppServices.SubHomeSerAppServices;
using HomeService.Domain.Services.CategoryServices;
using HomeService.Domain.Services.HomeServiceServices;
using HomeService.Domain.Services.SubHomeSerServices;
using Microsoft.Extensions.Caching.Memory;
using Serilog;
// سایر using ها...

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// کانفیگ لاگر Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("logs/api-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Services.AddSingleton<Serilog.ILogger>(Log.Logger);

// گرفتن رشته اتصال از فایل تنظیمات
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddMemoryCache();

// ثبت ریپازیتوری‌های Dapper با رشته اتصال
builder.Services.AddScoped<ICategoryRepository>(provider =>
    new CategoryDapperRepository(connectionString, Log.Logger));

builder.Services.AddScoped<IHomeServiceRepository>(provider =>
    new HomeServiceDapperRepository(connectionString, Log.Logger));

builder.Services.AddScoped<ISubHomeServiceRepository>(provider =>
    new SubHomeServiceDapperRepository(connectionString, Log.Logger));

// ثبت سایر سرویس‌ها
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICategoryAppService, CategoryAppService>();

builder.Services.AddScoped<IHomeServiceService, HomeServiceService>();
builder.Services.AddScoped<IHomeServiceAppService, HomeServiceAppService>();

builder.Services.AddScoped<ISubHomeServiceService, SubHomeServiceService>();
builder.Services.AddScoped<ISubHomeServiceAppService, SubHomeServiceAppService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();