
using Clinic_System.API.Extensions;
using Clinic_System.API.Middlewares;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Clinic_System.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("Logs/bootstrap-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
            try
            {
                Log.Information("Program Starting");
                var builder = WebApplication.CreateBuilder(args);

                // Serilog 
                builder.Host.UseSerilog((context, services, config) =>
                {
                    config.ReadFrom.Configuration(context.Configuration)
                    .ReadFrom.Services(services);
                });

                var connectionString = builder.Configuration.GetConnectionString("constr");
                builder.Services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseLazyLoadingProxies().UseSqlServer(connectionString);

                });

                // Add services to the container.

                builder.Services.AddControllers();
                // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
                builder.Services.AddOpenApi();

                builder.Services.AddIdenityServices(builder.Configuration);
                builder.Services.AddSwaggerDocumentation();

                var app = builder.Build();
                using (var scope = app.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    try
                    {
                        var context = services.GetRequiredService<AppDbContext>();
                        context.Database.Migrate();
                    }
                    catch (Exception ex)
                    {
                        var logger = services.GetRequiredService<ILogger<Program>>();
                        logger.LogError(ex, "An error occurred while migrating the database.");
                    }
                }

                app.UseMiddleware<ErrorHandlerMiddleware>();
                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.MapOpenApi();
                    app.UseSwagger();
                    app.UseSwaggerUI(c =>
                    {
                        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Elite Clinic");
                    });
                }
                app.UseHttpsRedirection();
                app.UseRouting();
                app.UseAuthentication();
                app.UseAuthorization();


                app.MapControllers();

                app.Run();

            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Program Stoped");
            }
            finally
            {
                Log.CloseAndFlush();
            }




        }
    }
}
