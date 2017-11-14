using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ggcvan.Data;
using ggcvan.Models;
using ggcvan.Services;
using Newtonsoft.Json.Linq;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ggcvan
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
            //var sqlConnectionString = Configuration.GetConnectionString("DataAccessMySqlProvider");
            
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
             .AddEntityFrameworkStores<ApplicationDbContext>()
             .AddDefaultTokenProviders();

            services.AddSingleton<IConfiguration>(Configuration);



            services.AddAuthentication().AddGoogle(googleOptions =>
            {
                var GoogleConfig = Configuration.GetSection("ExternalIdentities").GetSection("Google");
                //Console.WriteLine(GoogleConfig["client_id"]);


                googleOptions.ClientId = GoogleConfig["client_id"];
                googleOptions.ClientSecret = GoogleConfig["client_secret"];
            }).AddFacebook(facebookOptions =>
            {
                var FacebookConfig = Configuration.GetSection("ExternalIdentities").GetSection("Facebook");
                facebookOptions.AppId = FacebookConfig["app_id"];
                facebookOptions.AppSecret = FacebookConfig["app_secret"];
            })
            .AddCookie(cfg => cfg.SlidingExpiration = true)
            .AddJwtBearer(cfg =>
              {
                  cfg.RequireHttpsMetadata = false;
                  cfg.SaveToken = true;

                  cfg.TokenValidationParameters = new TokenValidationParameters()
                  {
                      ValidIssuer = Configuration["Tokens:Issuer"],
                      ValidAudience = Configuration["Tokens:Issuer"],
                      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Tokens:Key"]))
                  };

              });

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddMvc()
            .AddJsonOptions(
                options => {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                }
            );
            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "GGC VAN", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ApplicationDbContext ctx)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "GGC VAN API V1");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
                //routes.MapRoute()
            });

            DummyData.Initialize(ctx);
        }
    }
}
