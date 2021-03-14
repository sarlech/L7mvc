using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContosoUniversity.Data;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        public IConfiguration Configuration { get; }

        //tinfo200:[2021-03-12-costarec-dykstra2] -- Startup.cs uses this to add new services to the dependency injection container.
        public void ConfigureServices(IServiceCollection services)
        {
            //tinfo200:[2021-03-12-costarec-dykstra2] --  This code registers Schoolcontext as a services/adds it into the container.
            //tinfo200:[2021-03-12-costarec-dykstra2] --  The second part of the code "option => ..." indicates to the context what 
            // connection to use/ server to connect to-- in this case to a sql server which information is stored in a json file (because it's local).
            services.AddDbContext<SchoolContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //tinfo200:[2021-03-12-costarec-dykstra2] --  This code adds another service to the container which outputs useful error information
            //when exception occurs during execution
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddControllersWithViews();
        }

     
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
