using System;
using System.Threading.Tasks;
using Caffe.Models.ApiModels;
using Caffe.Models.Mapers;
using Caffe.Models.MongoModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
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
            services.AddIdentity<MongoUser, MongoRole>()
                .AddMongoDbStores<MongoUser, MongoRole, Guid>(new MongoDbContext(client.GetDatabase("users")));
            services.AddAuthentication().AddCookie();
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

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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