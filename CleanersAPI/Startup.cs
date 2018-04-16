using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanersAPI.Models;
using CleanersAPI.Repositories;
using CleanersAPI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CleanersAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CleanersApiContext>(options => options.UseSqlServer(Configuration.GetConnectionString("LocalConnection")));
            services.AddScoped<IProfessionalsService, ProfessionalsService>();
            services.AddScoped<IProfessionalsRepository, ProfessionalsRepository>();
            services.AddScoped<IProfessionsService, ProfessionsService>();
            services.AddScoped<IProfessionsRepository, ProfessionsRepository>();

            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }

//        public static class DbInitializer
//        {
//            public static void Initialize(IApplicationBuilder applicationBuilder)
//            {
//                using (var serviceScope = applicationBuilder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
//                {
//                    var context = serviceScope.ServiceProvider.GetService<CleanersApiContext>();
//                    context.Database.EnsureCreated();
//                    if (context.Persons.Any())
//                    {
//                        return;
//                    }
//
//                    var person = context.Persons.Add(new Person {FirstName = "Igwe", LastName = "Kabutindi"}).Entity;
//                    context.Persons.Add(person);
//                    context.SaveChanges();
//                }
//            }
//        }
    }
}
