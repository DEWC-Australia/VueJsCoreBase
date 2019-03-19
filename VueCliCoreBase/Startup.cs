using ASPIdentity.Data;
using Data.DatabaseLogger;
using Data.VueCoreBase;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Middleware.Exception;
using Middleware.Logger;
using Services.Email;
using System;
using System.Text;
using VueCliMiddleware;

namespace VueCliCoreBase
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

            services.AddMvc(options => { options.Filters.Add(new ApiExceptionFilter(_apiLogger)); })
                .AddJsonOptions(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
                .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_2_2);

            _logger.LogInformation("Added Jwt MVC with ApiExceptionFilter for ASP.NET Core Version_2_2 to services");

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                _logger.LogInformation("System is in development mode");
                app.UseDeveloperExceptionPage();
            }
            else
            {
                _logger.LogInformation("System is in Production mode");
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            _logger.LogInformation("System is using Https Redirection");

            app.UseCookiePolicy(); // optional GDPR
            _logger.LogInformation("System is using Cookie Policy");

            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseAuthentication();

            app.UseMiddleware<RequestResponseLoggingMiddleware>();

            _logger.LogInformation("System is using Request Response Logging");

            _logger.LogInformation("Starting Mvc");
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";
			
				if (env.IsDevelopment())
                {
                    // run npm process with client app
                    spa.UseVueCli(npmScript: "serve", port: 8080, regex: "Compiled ");
                }
            });
        }
    }
}
