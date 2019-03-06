using System;

using System.Text;
using ASPIdentity.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Middleware.Exception;
using Services.Email;
using Data.VueCoreBase;
using Microsoft.Extensions.Logging;
using Middleware.Logger;
using Data.DatabaseLogger;

namespace VueCoreBase
{
    public class Startup
    {
        private readonly ILogger _logger;
        private readonly ILogger<ApiExceptionFilter> _apiLogger;

        public Startup(IConfiguration configuration, ILogger<Startup> logger, ILogger<ApiExceptionFilter> apiLogger)
        {
            Configuration = configuration;
            _logger = logger;
            _apiLogger = apiLogger;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // optional GDPR compliant temaplates
            // This method gets called by the runtime. Use this method to add services to the container.
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
            _logger.LogInformation("Added Email sender to services");

            services.AddDbContext<ASPIdentityContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            _logger.LogInformation("Added ASPIdentityContext to services");

            services.AddDbContext<VueCoreBaseContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            _logger.LogInformation("Added VueCoreBaseContext to services");

            services.AddDbContext<DatabaseLoggerContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            _logger.LogInformation("Added DatabaseLoggerContext to services");

            services.AddIdentity<ApplicationUser, ApplicationRole>(
                opts =>
                {
                    opts.Password.RequireDigit = true;
                    opts.Password.RequireLowercase = true;
                    opts.Password.RequireUppercase = true;
                    opts.Password.RequireNonAlphanumeric = false;
                    opts.Password.RequiredLength = 8;

                    // Lockout settings
                    opts.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                    opts.Lockout.MaxFailedAccessAttempts = 5;

                    // User settings
                    opts.User.RequireUniqueEmail = true;

                    // User to confirm via work email
                    opts.SignIn.RequireConfirmedEmail = true;
                }
                )
            .AddEntityFrameworkStores<ASPIdentityContext>()
            .AddDefaultTokenProviders();

            _logger.LogInformation("Added AddIdentity to services with the following Password Restrictions: {RequireDigit, RequireLowercase, RequireUppercase, RequiredLength, Lockout 30 min after 5 attempts, Confirm email required} ");

            // Add ASP.NET Identity support
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.ClaimsIssuer = Configuration["Authentication:JwtIssuer"];

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = Configuration["Authentication:JwtIssuer"],

                        ValidateAudience = true,
                        ValidAudience = Configuration["Authentication:JwtAudience"],

                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Authentication:JwtKey"])),

                        RequireExpirationTime = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            _logger.LogInformation("Added Jwt Authenication to services");

            services.AddMvc(options => {options.Filters.Add(new ApiExceptionFilter(_apiLogger));})
                .AddJsonOptions( options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
                .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_2_2);

            _logger.LogInformation("Added Jwt MVC with ApiExceptionFilter for ASP.NET Core Version_2_2 to services");

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
             if (env.IsDevelopment())
            {
                _logger.LogInformation("System is in development mode");
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true
                });
            }
            else
            {
                _logger.LogInformation("System is in Production mode");

                app.UseExceptionHandler("/Home/Error");

                app.UseHsts();
            }
            app.UseHttpsRedirection();

            _logger.LogInformation("System is using Https Redirection");

            app.UseCookiePolicy(); // optional GDPR
            _logger.LogInformation("System is using Cookie Policy");

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMiddleware<RequestResponseLoggingMiddleware>();

            _logger.LogInformation("System is using Request Response Logging");


            _logger.LogInformation("Starting Mvc");

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");


                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );


                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });
        }
    }
}
