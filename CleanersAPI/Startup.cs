using System;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using CleanersAPI.Models;
using CleanersAPI.Repositories;
using CleanersAPI.Repositories.Impl;
using CleanersAPI.Services;
using CleanersAPI.Services.Impl;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
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
            var key = Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value);
            services.AddDbContext<CleanersApiContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
//            services.AddDbContext<CleanersApiContext>(options =>
//                options.UseMySql(Configuration.GetConnectionString("RemoteMysql")));

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .Build());
            });
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<DbContext, CleanersApiContext>();
            services.AddScoped<IProfessionalsService, ProfessionalsService>();
            services.AddScoped<IProfessionalsRepository, ProfessionalsRepository>();

            services.AddScoped<IProfessionsService, ProfessionsService>();
            services.AddScoped<IProfessionsRepository, ProfessionsRepository>();

            services.AddScoped<ICustomersService, CustomersService>();
            services.AddScoped<ICustomersRepository, CustomersRepository>();

            services.AddScoped<IEmailsService, EmailsService>();
            services.AddScoped<IEmailsRepository, EmailsRepository>();

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAuthRepository, AuthRepository>();

            services.AddScoped<IUsersRepository, UsersRepository>();

            services.AddScoped<IExpertiseService, ExpertiseService>();
            services.AddScoped<IExpertiseRepository, ExpertiseRepository>();

            services.AddScoped<IServicesService, ServicesService>();
            services.AddScoped<IServicesRepository, ServicesRepository>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false, //Read about Issuer and Audience
                    ValidateAudience = false
                };
            });

            services.AddMvc().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

//            app.UseCors("CorsPolicy");
            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            app.UseAuthentication();
             //app.UseMvc();
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

                    context.Professions.Add(new Service
                    {
                        Title = "Montage meubles",
                        Description = "Montage blablaaaa...",
                        Category = Category.Interieur
                    });
                    context.Professions.Add(new Service
                    {
                        Title = "Ménage",
                        Description = "Les travaux ménagers etc",
                        Category = Category.Interieur
                    });
                    context.Professions.Add(new Service {Title = "Peinture", Category = Category.Interieur});

                    CreatePasswordHash("password", out var passwordHash, out var passwordSalt);
                    var admin = new User {Username = "admin", PasswordHash = passwordHash, PasswordSalt = passwordSalt};
                    var igwe = new User {Username = "igwe", PasswordHash = passwordHash, PasswordSalt = passwordSalt};
                    var adminRoleUser = new RoleUser {role = new Role {RoleName = RoleName.Admin}, user = admin};
                    var userRoleUser = new RoleUser {role = new Role {RoleName = RoleName.User}, user = igwe};
                    var profession = new Service {Title = "Gutera akabariro", Category = Category.Interieur};
                    var professional = new Professional
                    {
                        Address = new Address
                        {
                            City = "Schaerbeek",
                            StreetName = "Rue Gaucheret",
                            PostalCode = "1030",
                            PlotNumber = "4"
                        },
                        FirstName = "Igwe",
                        LastName = "Kabutindi",
                        Email = "medard.rebero@gmail.com",
                        Phone = "+32483378014",
                        User = igwe
                    };

                    var expertise = new Expertise
                    {
                        Service = profession,
                        Professional = professional,
                        HourlyRate = 500
                    };

                    context.Users.Add(admin);
                    context.Expertises.Add(expertise);
                    context.RoleUsers.Add(adminRoleUser);
                    context.RoleUsers.Add(userRoleUser);
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
            using (var hmac = new HMACSHA512()) //Because HMACSHA512() implements IDisposable
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
    }
}