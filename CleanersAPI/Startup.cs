using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CleanersAPI.Models;
using CleanersAPI.Repositories;
using CleanersAPI.Repositories.Impl;
using CleanersAPI.Services;
using CleanersAPI.Services.Impl;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

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
            //services.AddDbContext<CleanersApiContext>(options => options.UseSqlServer(Configuration.GetConnectionString("LocalConnection2")));
            services.AddDbContext<CleanersApiContext>(options =>
                options.UseMySql(Configuration.GetConnectionString("RemoteMysql")));

            services.AddScoped<DbContext, CleanersApiContext>();
            services.AddScoped<IProfessionalsService, ProfessionalsService>();
            services.AddScoped<IProfessionalsRepository, ProfessionalsRepository>();

            services.AddScoped<IProfessionsService, ProfessionsService>();
            services.AddScoped<IProfessionsRepository, ProfessionsRepository>();

            services.AddScoped<ICustomersService, CustomersService>();
            services.AddScoped<ICustomersRepository, CustomersRepository>();

            services.AddScoped<IEmailsService, EmailsService>();
            services.AddScoped<IEmailsRepository, EmailsRepository>();

            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                DbInitializer.Initialize(app);
            }
            else
            {
                app.UseExceptionHandler(builder =>
                {
                    builder.Run(async context =>
                    {
                        context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                        var error = context.Features.Get<IExceptionHandlerFeature>();
                        if (error != null)
                        {
//                            context.Response.AddApplicationError(error.Error.Message);
                            await context.Response.WriteAsync(error.Error.Message);
                        }
                    });
                });
            }

            app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().AllowCredentials());
            app.UseMvc();
        }

        private static class DbInitializer
        {
            public static void Initialize(IApplicationBuilder applicationBuilder)
            {
                using (var serviceScope = applicationBuilder.ApplicationServices
                    .GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    var context = serviceScope.ServiceProvider.GetService<CleanersApiContext>();
                    context.Database.EnsureCreated();
                    if (context.Users.Any())
                    {
                        return;
                    }

                    context.Professions.Add(new Profession {Title = "Montage meubles", Description = "Montage blablaaaa...",Category = Category.Bricolage});
                    context.Professions.Add(new Profession {Title = "Ménage", Description = "Les travaux ménagers etc",Category = Category.Cleaning});
                    context.Professions.Add(new Profession {Title = "Peinture", Category = Category.Construction});

                    CreatePasswordHash("password", out var passwordHash, out var passwordSalt);
                    var admin = new User {Username = "Admin", PasswordHash = passwordHash, PasswordSalt = passwordSalt};
                    var profession = new Profession {Title = "Gutera akabariro", Category = Category.Construction};
                    var professional = new Professional
                    {
                        Address = new Address
                        {
                            Commune = "Schaerbeek",
                            Street = "Rue Gaucheret",
                            Zipcode = "1030",
                            Number = "4"
                        },
                        FirstName = "Igwe",
                        LastName = "Kabutindi",
                        RegNumber = "PRO_" + GenerateRegistrationNumber(10000, 90000),
                        User = new User
                        {
                            Username = "Igwe",
                            PasswordHash = passwordHash,
                            PasswordSalt = passwordSalt
                        }
                    };

                    var expertise = new Expertise
                    {
                        Profession = profession,
                        Professional = professional,
                        UnitPrice = 500
                    };

                    context.Users.Add(admin);
                    context.Expertises.Add(expertise);
                    context.SaveChanges();
                }
            }
        }

        private static int GenerateRegistrationNumber(int min, int max)  
        {  
            var random = new Random();  
            return random.Next(min, max);  
        }
        
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512()) //Because HMACSHA512() implements IDisposable
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}