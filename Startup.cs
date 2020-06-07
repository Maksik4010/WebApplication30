using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using WebApplication30.Data;
using WebApplication30.Data.Managers;
using WebApplication30.Models;
using WebApplication30.Repositories;
using WebApplication30.Service.Contract;
using WebApplication30.Service.Implementation;

namespace WebApplication30
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
            
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // User, Authentication, Authorization
            services.AddIdentity<AppUser, AppRole>(o =>
            {
                o.Password.RequireDigit = false;
                o.Password.RequireLowercase = true;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequireUppercase = false;
                o.Password.RequiredLength = 8;
            })
                .AddRoles<AppRole>()
                .AddUserStore<UserRepository>()
                .AddRoleStore<RoleRepository>()
                .AddUserManager<AppUserManager>()
                .AddRoleManager<AppRoleManager>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddAuthentication(options =>
            {
                // Identity made Cookie authentication the default.
                // However, we want JWT Bearer Auth to be the default.
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
              .AddJwtBearer(options =>
              {
                  // Configure the Authority to the expected value for your authentication provider
                  // This ensures the token is appropriately validated
                  options.Authority = "https://webapplication3020200606083646.azurewebsites.net/user/token";

                  // We have to hook the OnMessageReceived event in order to
                  // allow the JWT authentication handler to read the access
                  // token from the query string when a WebSocket or 
                  // Server-Sent Events request comes in.

                  // Sending the access token in the query string is required due to
                  // a limitation in Browser APIs. We restrict it to only calls to the
                  // SignalR hub in this code.
                  // See https://docs.microsoft.com/aspnet/core/signalr/security#access-token-logging
                  // for more information about security considerations when using
                  // the query string to transmit the access token.
                  options.Events = new JwtBearerEvents
                  {
                      OnMessageReceived = context =>
                      {
                          var accessToken = context.Request.Query["Bearer"];

                          // If the request is for our hub...
                          var path = context.HttpContext.Request.Path;
                          if (!string.IsNullOrEmpty(accessToken) &&
                              (path.StartsWithSegments("/book")))
                          {
                              // Read the token out of the query string
                              context.Token = accessToken;
                          }
                          return Task.CompletedTask;
                      }
                  };
              });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            
            // Repositories
            services.AddTransient<BookReposiotry>();

            // Seevices
            services.AddTransient<IAppRoleService, AppRoleService>();
            services.AddTransient<IGlobalUserService, GlobalUserService>();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                //The generated Swagger JSON file will have these properties.
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Libary API",
                    Version = "v1",
                });

                //Locate the XML file being generated by ASP.NET...
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.XML";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                //... and tell Swagger to use those XML comments.
                c.IncludeXmlComments(xmlPath);

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description =
                        "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseDeveloperExceptionPage();
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvcWithDefaultRoute();
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            // ConfigureApplication(serviceProvider).Wait();
        }

        // This method was created to check some custom conditions and do some custom configuration in app during server starting.
        //public async Task ConfigureApplication(IServiceProvider serviceProvider)
        //{
        //    // create roles if not exists in app database
        //    var applicationRoleManager = serviceProvider.GetService<IAppRoleService>();
        //    await applicationRoleManager.EnsureAppRolesExists();

        //    // Configure global administrator
        //    var globalUserService = serviceProvider.GetService<IGlobalUserService>();
        //    await globalUserService.EnsureGlobalUserExists();
        //    await globalUserService.EnsureGlobalUserHasGlobalRoles();
        //}
    }
}
