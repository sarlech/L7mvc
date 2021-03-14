using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContosoUniversity.Data;
using Microsoft.Extensions.DependencyInjection;

namespace ContosoUniversity
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //CreateHostBuilder(args).Build().Run();

            //tinfo200:[2021-03-11-costarec-dykstra2] --  create a web host and initialize it by calling the method Build() on the
            //obejct/host returned by the function CreateHostBuilder.
            var host = CreateHostBuilder(args).Build();

            //tinfo200:[2021-03-11-costarec-dykstra2] --  pass the created and built host to CreateDBIfNotExists() function to check
            //if the databse component was initialized. IF no database was found it then creates one.
            CreateDbIfNotExists(host);

            //tinfo200:[2021-03-11-costarec-dykstra2] --  Run the host, hence all the parts of the web appliation as needed.
            host.Run();
        }

        //tinfo200:[2021-03-11-costarec-dykstra2] --  Check if the host database was initialized. If no, initialize it.
        private static void CreateDbIfNotExists(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                //tinfo200:[2021-03-11-costarec-dykstra2] --  get a sort of list of all services in the container of the host.
                var services = scope.ServiceProvider;
                try
                {
                    // //tinfo200:[2021-03-11-costarec-dykstra2] --  use this code to capture and store the context (SchoolContext)
                    var context = services.GetRequiredService<SchoolContext>();

                    //tinfo200:[2021-03-11-costarec-dykstra2] --  create the Bb if doesn't exist.
                    DbInitializer.Initialize(context);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred creating the DB.");
                }
            }
        }

        //tinfo200:[2021-03-11-costarec-dykstra2] --  This method is suppposed to create an host by implementing the interface method IHostBuilder.
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
