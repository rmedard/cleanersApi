﻿using System;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using CleanersAPI.Helpers;
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
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace CleanersAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var key = Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value);
            services.AddDbContext<CleanersApiContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        // .AllowCredentials()
                        .Build());
            });
            services.AddSingleton(provider => new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfiles(provider.GetService<IConfiguration>()));
            }).CreateMapper());
            services.AddAutoMapper(typeof(AutoMapperProfiles));

            services.AddScoped<DbContext, CleanersApiContext>();

            services.AddScoped<IBillingsService, BillingsService>();
            services.AddScoped<IBillingsRepository, BillingsRepository>();

            services.AddScoped<IProfessionalsService, ProfessionalsService>();
            services.AddScoped<IProfessionalsRepository, ProfessionalsRepository>();

            services.AddScoped<IServicesService, ServicesService>();
            services.AddScoped<IServicesRepository, ServicesRepository>();

            services.AddScoped<ICustomersService, CustomersService>();
            services.AddScoped<ICustomersRepository, CustomersRepository>();

            services.AddScoped<IEmailsService, EmailsService>();
            services.AddScoped<IEmailsRepository, EmailsRepository>();

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAuthRepository, AuthRepository>();

            services.AddScoped<IUsersRepository, UsersRepository>();

            services.AddScoped<IExpertiseService, ExpertiseService>();
            services.AddScoped<IExpertiseRepository, ExpertiseRepository>();

            services.AddScoped<IReservationsService, ReservationsService>();
            services.AddScoped<IReservationsRepository, ReservationsRepository>();

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

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "HomeCleaners API", Version = "v1"
                });
            });

            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
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
                            await context.Response.WriteAsync(error.Error.Message);
                        }
                    });
                });
            }

            app.UseCors("CorsPolicy");

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "HouseCleaners API");
            });
            
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        private static class DbInitializer
        {
            public static void Initialize(IApplicationBuilder applicationBuilder)
            {
                using var serviceScope = applicationBuilder.ApplicationServices
                    .GetRequiredService<IServiceScopeFactory>().CreateScope();
                var context = serviceScope.ServiceProvider.GetService<CleanersApiContext>();
                context.Database.EnsureCreated();
                if (context.Users.Any())
                {
                    return;
                }

                context.Services.Add(new Service
                {
                    Title = "Montage meubles",
                    Description = "Montage blablaaaa...",
                    Category = Category.Interieur
                });
                context.Services.Add(new Service
                {
                    Title = "Ménage",
                    Description = "Les travaux ménagers etc",
                    Category = Category.Interieur
                });
                context.Services.Add(new Service
                {
                    Title = "Peinture",
                    Category = Category.Interieur
                });

                CreatePasswordHash("password", out var passwordHash, out var passwordSalt);
                var admin = new User {Username = "admin@gmail.com", PasswordHash = passwordHash, PasswordSalt = passwordSalt};
                var igwe = new User {Username = "igwe@gmail.com", PasswordHash = passwordHash, PasswordSalt = passwordSalt};
                var adminRoleUser = new RoleUser {Role = new Role {RoleName = RoleName.Admin}, User = admin};
                var professionalRoleUser = new RoleUser {Role = new Role {RoleName = RoleName.Professional}, User = igwe};
                var service = new Service {Title = "Gutera akabariro", Category = Category.Interieur};
                var professional = new Professional
                {
                    Address = new Address
                    {
                        City = "Schaerbeek",
                        StreetName = "Rue Gaucheret",
                        PostalCode = 1030,
                        PlotNumber = "4"
                    },
                    FirstName = "Igwe",
                    LastName = "Kabutindi",
                    Email = igwe.Username,
                    PhoneNumber = "+32483378014",
                    User = igwe
                };

                var expertise = new Expertise
                {
                    Service = service,
                    Professional = professional,
                    HourlyRate = new decimal(50.00)
                };

                context.Roles.Add(new Role {RoleName = RoleName.Customer});
                context.Users.Add(admin);
                context.Expertises.Add(expertise);
                context.RoleUser.Add(adminRoleUser);
                context.RoleUser.Add(professionalRoleUser);
                context.SaveChanges();
            }
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
    }
}