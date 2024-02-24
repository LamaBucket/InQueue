using Microsoft.AspNetCore.Authentication.Cookies;
using SQA.Domain;
using Integrated.Loggers;
using SQA.Domain.Services;
using SQA.Domain.Services.Data;
using SQA.EntityFramework;
using SQA.EntityFramework.Services;
using SQA.Web;
using SQA.Web.Middlewares;
using Integrated.Loggers.State;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
var logger = CreateAppLogger();

builder.Services.AddSingleton(logger);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddLogging();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.Cookie.Name = "Authentication";
});
builder.Services.AddAuthorization();

builder.Services.AddTransient<SQADbContextFactory>();

builder.Services.AddScoped<IUserDataService, UserDataService>();
builder.Services.AddScoped<IQueueDataService, QueueDataService>();

builder.Services.AddScoped<IStringHasher, PasswordHasher>();
builder.Services.AddScoped<IUserBuilder, UserBuilder>();
builder.Services.AddScoped<IUserPasswordProvider, UserPasswordProvider>();

builder.Services.AddScoped<IQueueBuilder, QueueBuilder>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseMiddleware<LoggerMiddleware>();

app.Run();

ICustomLogger CreateAppLogger()
{
    string currentDir = Directory.GetCurrentDirectory();

    FileLoggerDataService informationService = new(CustomLogLevel.Information, Path.Combine(currentDir, "infos.txt"));
    FileLoggerDataService errorService = new(CustomLogLevel.Error, Path.Combine(currentDir, "errors.txt"));


    Logger logger = new();
    logger.AddDataService(informationService);
    logger.AddDataService(errorService);

    return logger;
}