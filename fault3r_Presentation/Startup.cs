using fault3r_Application.Interfaces;
using fault3r_Application.Services.AccountsRepository;
using fault3r_Application.Services.AccountRepository;
using fault3r_Persistence.Contexts;
using fault3r_Presentation.Models.Validators.Accounts;
using fault3r_Presentation.Models.ViewModels.Accounts;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using fault3r_Presentation.Middlewares;
using fault3r_Application.Services.LoggingService;
using fault3r_Presentation.Models.ViewModels.Account;
using fault3r_Presentation.Models.Validators.Account;
using fault3r_Application.Services.UsersRepository;
using fault3r_Application.Services.ForumsRepository;
using fault3r_Presentation.Areas.AdminPanel.Models.ViewModels.Forums;
using fault3r_Presentation.Areas.AdminPanel.Models.Validators.Forums;
using System.Globalization;

namespace fault3r_Presentation
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
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.DefaultThreadCurrentUICulture
                = new CultureInfo("fa-IR");
            
            services.AddScoped<IDatabaseContext, DatabaseContext>();
            services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("sqldb"));
            });

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
                .AddCookie(options =>
                {
                    options.LoginPath = new PathString("/Accounts/SignIn");
                    options.AccessDeniedPath = new PathString("/Error/403");
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("requireAdmin", policy => policy.RequireRole("ADMIN"));
                options.AddPolicy("requireAccount", policy => policy.RequireRole("ACCOUNT"));
            });

            services.AddScoped<ILoggingService,LoggingService>();

            services.AddScoped<IAccountsRepository, AccountsRepository>();
            services.AddScoped<IValidator<SignUpViewModel>, SignUpValidator>();
            services.AddScoped<IValidator<SignInViewModel>, SignInValidator>();

            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IValidator<UpdateAccountViewModel>, UpdateAccountValidator>();
            services.AddScoped<IValidator<ChangePasswordAccountViewModel>, ChangePasswordAccountValidator>();
            services.AddScoped<IValidator<DeleteAccountViewModel>, DeleteAccountValidator>();

            services.AddScoped<IUsersRepository, UsersRepository>();

            services.AddScoped<IForumsRepository, ForumsRepository>();
            services.AddScoped<IValidator<AddForumViewModel>, AddForumValidator>();
            services.AddScoped<IValidator<EditForumViewModel>,EditForumValidator>();
            services.AddScoped<IValidator<DeleteForumViewModel>, DeleteForumValidator>();

            services.AddHttpContextAccessor();

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {                        
            app.UseStatusCodeHandler();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
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
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
