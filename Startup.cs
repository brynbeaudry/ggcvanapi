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
using AspNet.Security.OpenIdConnect.Primitives;

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
                {
                    options.UseMySql(Configuration.GetConnectionString("DefaultConnection"));
                    options.UseOpenIddict();
                }
            );

            services.AddIdentity<ApplicationUser, IdentityRole>((options =>
                {
                    // Password settings
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 4; 
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = true;
                    options.Password.RequiredUniqueChars = 2;
                }))
             .AddEntityFrameworkStores<ApplicationDbContext>()
             .AddDefaultTokenProviders();

             // Configure Identity to use the same JWT claims as OpenIddict instead
            // of the legacy WS-Federation claims it uses by default (ClaimTypes),
            // which saves you from doing the mapping in your authorization controller.
            services.Configure<IdentityOptions>(options =>
            {
                options.ClaimsIdentity.UserNameClaimType = OpenIdConnectConstants.Claims.Name;
                options.ClaimsIdentity.UserIdClaimType = OpenIdConnectConstants.Claims.Subject;
                //options.ClaimsIdentity.RoleClaimType = OpenIdConnectConstants.Claims.Role;
            });

            

            services.AddSingleton<IConfiguration>(Configuration);
            
            // Register the OAuth2 validation handler.
            services.AddAuthentication()
            .AddGoogle(googleOptions =>
            {
                var GoogleConfig = Configuration.GetSection("ExternalIdentities").GetSection("Google");
                //Console.WriteLine(GoogleConfig["client_id"]);


                googleOptions.ClientId = GoogleConfig["client_id"];
                googleOptions.ClientSecret = GoogleConfig["client_secret"];
            })
            .AddFacebook(facebookOptions =>
            {
                var FacebookConfig = Configuration.GetSection("ExternalIdentities").GetSection("Facebook");
                facebookOptions.AppId = FacebookConfig["app_id"];
                facebookOptions.AppSecret = FacebookConfig["app_secret"];
            })
            .AddOAuthValidation();
            /* .AddCookie(cfg => cfg.SlidingExpiration = true)
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

              }); */

            // Register the OpenIddict services.
            services.AddOpenIddict(options =>
            {
                // Register the Entity Framework stores.
                options.AddEntityFrameworkCoreStores<ApplicationDbContext>();

                // Register the ASP.NET Core MVC binder used by OpenIddict.
                // Note: if you don't call this method, you won't be able to
                // bind OpenIdConnectRequest or OpenIdConnectResponse parameters.
                options.AddMvcBinders();

                // Enable the authorization, logout, token and userinfo endpoints.
                options.EnableAuthorizationEndpoint("/connect/authorize")
                       .EnableLogoutEndpoint("/connect/logout")
                       .EnableTokenEndpoint("/connect/token")
                       .EnableUserinfoEndpoint("/api/userinfo");
                // Note: the Mvc.Client sample only uses the code flow and the password flow, but you
                // can enable the other flows if you need to support implicit or client credentials.
                options.AllowAuthorizationCodeFlow()
                       .AllowPasswordFlow()
                       //.AllowImplicitFlow()
                       .AllowRefreshTokenFlow()
                       //warning!!!!!! configure allowed audiences or clients
                       .AllowCustomFlow("urn:ietf:params:oauth:grant-type:google_identity_token")
                       .AllowCustomFlow("urn:ietf:params:oauth:grant-type:facebook_access_token");



                // Mark the "profile" scope as a supported scope in the discovery document.
                options.RegisterScopes(OpenIdConnectConstants.Scopes.Profile);

                // Make the "client_id" parameter mandatory when sending a token request.
                //options.RequireClientIdentification();

                // When request caching is enabled, authorization and logout requests
                // are stored in the distributed cache by OpenIddict and the user agent
                // is redirected to the same page with a single parameter (request_id).
                // This allows flowing large OpenID Connect requests even when using
                // an external authentication provider like Google, Facebook or Twitter.
                options.EnableRequestCaching();

                // During development, you can disable the HTTPS requirement.
                options.DisableHttpsRequirement();

                // Note: to use JWT access tokens instead of the default
                // encrypted format, the following lines are required:
                //
                // options.UseJsonWebTokens();
                // options.AddEphemeralSigningKey();
            });

             var policy = new Microsoft.AspNetCore.Cors.Infrastructure.CorsPolicy();
 
                policy.Headers.Add("*");
                policy.Methods.Add("*");
                policy.Origins.Add("*");
                policy.SupportsCredentials = true;
            
            services.AddCors(x => x.AddPolicy("corsGlobalPolicy", policy));

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

            services.AddTransient<IEmailSender, EmailSender>();
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

            app.UseCors("corsGlobalPolicy");

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
