using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Caffe.Models;
using Caffe.Models.ApiModels;
using Caffe.Models.Mappers;
using Caffe.Models.MongoModels;
using Caffe.Models.PasswordValidators;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using MongoDbGenericRepository;

namespace Caffe
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            var settings =
                MongoClientSettings.FromConnectionString(Configuration.GetConnectionString("DefaultConnection"));
            var client = new MongoClient(settings);
            services.AddSingleton(client);

            services.AddSingleton(new Mail(Configuration.GetConnectionString("Email"),
                Configuration.GetConnectionString("EmailPass"), "smtp.gmail.com", 587));
            services.AddSingleton(new Dictionary<string, UserDto>());
            services.AddIdentity<MongoUser, MongoRole>(options =>
                {
                    options.Password.RequiredLength = 0;
                    options.Password.RequireDigit = false;
                    options.Password.RequiredUniqueChars = 0;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                })
                .AddMongoDbStores<MongoUser, MongoRole, Guid>(new MongoDbContext(client.GetDatabase("users")))
                .AddPasswordValidator<PasswordLengthValidator>();
            services.AddAuthentication().AddCookie().AddGoogle(options =>
            {
                options.ClientId = Configuration.GetConnectionString("ClientId");
                options.ClientSecret = Configuration.GetConnectionString("ClientSecret");
            });

            services.ConfigureApplicationCookie(ops =>
            {
                ops.Cookie.Name = "Caffe_cookie";
                ops.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Task.CompletedTask;
                };
            });
            services.AddAutoMapper(x => x.AddProfile<MenuMappingProfile>());
            services.AddControllersWithViews();
            services.AddSpaStaticFiles(configuration => { configuration.RootPath = "ClientApp/build"; });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}