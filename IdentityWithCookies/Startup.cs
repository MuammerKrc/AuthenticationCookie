using IdentityWithCookies.Claimss;
using IdentityWithCookies.CustomValidator;
using IdentityWithCookies.EmailService;
using IdentityWithCookies.Helper;
using IdentityWithCookies.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityWithCookies
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
            /*Configure örnekleyerek kullanmak*/
            services.AddDbContext<ApplicationContext>(opt =>
            {
                opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("IstanbulPolicy", policy =>
                {
                    policy.RequireClaim("city", "İstanbul");
                });
            });

            services.AddIdentity<User, Role>().AddPasswordValidator<PasswordCustomValidator>().AddUserValidator<UserCustomValidator>().AddErrorDescriber<CustomIdentityErrorDescriber>().AddEntityFrameworkStores<ApplicationContext>().AddDefaultTokenProviders();
            services.Configure<IdentityOptions>(opt =>
            {
                //Password
                opt.Password.RequireDigit = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequiredLength = 4;

                //User
                opt.User.AllowedUserNameCharacters += "çiüö";
                opt.User.RequireUniqueEmail = true;
            });
           


            #region EmailService
            services.AddScoped<IEmailService, IdentityWithCookies.EmailService.EmailService>(i => new EmailService.EmailService
            (
                Configuration.GetValue<int>("EmailService:Port"),
                Configuration["EmailService:Host"],
                Configuration.GetValue<bool>("EmailService:EnableSSl"),
                Configuration["EmailService:UserName"],
                Configuration["EmailService:Password"]
            ));

           
            #endregion
            //Cookies Configure
            CookieBuilder cookieBuilder = new CookieBuilder();

            cookieBuilder.HttpOnly = false;
            cookieBuilder.Name = "KrcBlog";
            cookieBuilder.SameSite = SameSiteMode.Lax;
            cookieBuilder.SecurePolicy = CookieSecurePolicy.SameAsRequest;
            services.ConfigureApplicationCookie(opt =>
            {
                opt.LoginPath = new PathString("/Home/Login");
                opt.LogoutPath = new PathString("/Member/LogOut");
                opt.SlidingExpiration = true;
                opt.Cookie = cookieBuilder;
                opt.ExpireTimeSpan = TimeSpan.FromDays(60);
                opt.AccessDeniedPath = new PathString("/Member/AccessDenied");
            });
            services.AddScoped<HelperMethods>();
            services.AddScoped<IClaimsTransformation, ClaimProvider>();


            services.AddControllersWithViews();
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
