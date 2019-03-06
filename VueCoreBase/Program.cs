using Data.DatabaseLogger;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Middleware.DatabaseLogger;


namespace VueCoreBase
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            

            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureLogging((hostingContext, logging) =>
                {

                    DbContextOptionsBuilder<DatabaseLoggerContext> options = new DbContextOptionsBuilder<DatabaseLoggerContext>();
                   
                    logging.AddProvider(new DatabaseLoggerProvider(
                        new DatabaseLoggerConfiguration { LogLevel = LogLevel.Warning, EnableConsole = false, EventId = 0 },
                        new DatabaseLoggerContext(options.UseSqlServer(hostingContext.Configuration.GetConnectionString("DefaultConnection")).Options),
                        hostingContext.HostingEnvironment.ApplicationName,
                        hostingContext.HostingEnvironment.EnvironmentName)
                        );
                }).Build();
        }
            
    }
}
