using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SportsStore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace SportsStore
{
    public class Startup
    {
        private readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        private readonly string DbConnectionString = "SportsStoreConnection";
        private readonly string IdentityConnectionString = "IdentityConnection";
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureDependencies(services);
            ConfigureDatabase(services);
            ConfigureCors(services);
            services.AddControllers()
                .AddNewtonsoftJson(o => {
                    o.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
                    o.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                });
            

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            app.UseCors(MyAllowSpecificOrigins);
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
          
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
        }
        private void ConfigureDependencies(IServiceCollection services)
        {
            services.AddScoped<DataContext, DataContext>();
        }

        private void ConfigureDatabase(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString(DbConnectionString)));

        }

        private void ConfigureCors(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                    policy =>
                    {
                        policy.AllowCredentials()
                            .WithOrigins(
                            "http://localhost:1998")
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                        //policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                    });
            });
        }

    }
}
