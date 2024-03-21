using Microsoft.AspNetCore.Authentication.Cookies;
using SQA.Domain;
using Integrated.Loggers;
using SQA.Domain.Services;
using SQA.Domain.Services.Data;
using SQA.EntityFramework;
using SQA.EntityFramework.Services;
using SQA.Web;
using SQA.Web.Middlewares;
using SQA.Web.Controllers;
using Microsoft.AspNetCore.SignalR;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

        builder.Services.AddLogging(builder =>
        {
            builder.AddFile();
        });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddControllers();
        builder.Services.AddMvc();

        builder.Services.AddSingleton<IUserIdProvider, HubUserProvider>();

        builder.Services.AddSignalR(x => x.EnableDetailedErrors = true);

        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
        {
            options.Cookie.Name = "Authentication";
            options.AccessDeniedPath = "/Authenticate";
            options.LoginPath = "/Login";
        });
        builder.Services.AddAuthorization();

        builder.Services.AddTransient<SQADbContextFactory>();

        builder.Services.AddScoped<IUserDataService, UserDataService>();
        builder.Services.AddScoped<IQueueDataService, QueueDataService>();
        builder.Services.AddScoped<IUserRoleDataService, UserRoleDataService>();

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
        app.UseStaticFiles();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        app.UseMiddleware<LoggerMiddleware>();

        app.MapHub<QueueHub>("/queue");

        app.MapControllerRoute(
            name: "Default",
            "{controller}",
            new { controller = "WebApp" });

        app.Run();
    }
}

public class HubUserProvider : IUserIdProvider
{
    public string? GetUserId(HubConnectionContext connection)
    {
        return connection.User.Identity?.Name;
    }
}