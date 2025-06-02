using System;
using System.Reflection;
using System.IO;
using Microsoft.EntityFrameworkCore;
using dockerapi.Models;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Swashbuckle.AspNetCore.SwaggerUI;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Models;
using Npgsql;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

// Получаем строку подключения из переменной окружения
var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
Console.WriteLine($"DB_CONNECTION_STRING = {connectionString}");

// Добавление DbContext (один раз)
builder.Services.AddDbContext<ApiDbContext>(options =>
    options.UseNpgsql(connectionString));

// Добавление контроллеров
builder.Services.AddControllers()
    .AddMvcOptions(options => options.EnableEndpointRouting = false);

// Добавление Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Web API",
        Version = "v1",
        Description = "ASP.NET Core Web API with Docker and PostgreSQL",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "John Smith"
        }
    });

    // Подключение XML-комментариев
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// Конфигурация пайплайна
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Swagger middleware
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web API V1");
});

// Маршрутизация на контроллеры
app.MapControllers();

// Применение миграций с повторными попытками
using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<ApiDbContext>();

int retryCount = 30;
int delayMs = 1000;

for (int i = 0; i < retryCount; i++)
{
   try
    {
        Console.WriteLine("Попытка подключения к БД...");
        dbContext.Database.Migrate();
        Console.WriteLine("Подключение к БД успешно");
        break;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ошибка подключения к БД: {ex.Message}");
        Console.WriteLine($"InnerException: {ex.InnerException?.Message}");
        await Task.Delay(delayMs);
    }
}

// Запуск приложения
app.Run();