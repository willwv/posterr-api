using Application.QueriesHandler;
using Domain.Interfaces.Repositories;
using Infrastructure.Databases.MySql;
using Infrastructure.Databases.MySql.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Diagnostics.CodeAnalysis;

namespace WebApi.Extensions
{
    [ExcludeFromCodeCoverage]
    internal static class Program
    {
        static void Main(string[] args) 
        { 
            var builder = WebApplication.CreateBuilder(args);
            var MySqlConnectionString = Environment.GetEnvironmentVariable("SQL_CONNECTION_STRING");

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Posterr API",
                    Version = "v1",
                    Description = ".NET Core API for Posterr",
                });

                c.EnableAnnotations();
            });
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<GetAllPostsQueryHandler>());
            builder.Services.AddDbContext<MySqlContext>(options => options.UseMySql(MySqlConnectionString, ServerVersion.AutoDetect(MySqlConnectionString)));
            builder.Services.AddScoped<IPostRepository, PostRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();


            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Posterr API V1");
            });
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.ApplyMySqlMigrations();
            app.SeedMySqlDatabase();
            app.Run();
        }
    }
}