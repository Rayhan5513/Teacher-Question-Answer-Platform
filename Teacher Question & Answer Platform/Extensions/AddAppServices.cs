using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using TeacherStudentQAPlatform.Data;
using TeacherStudentQAPlatform.Services;

namespace TeacherStudentQAPlatform.Extensions
{
    public static class AddAppServices
    {
        public static void AddDbServices(this IServiceCollection services, WebApplicationBuilder builder)
        {
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

            // For authentication 
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(options =>
            {
                options.Cookie.Name = TSQADefaults.AspToken;
                options.LoginPath = TSQADefaults.LoginUrl;
                options.LogoutPath = TSQADefaults.LogoutUrl;
                options.AccessDeniedPath = TSQADefaults.AccessDeniedUrl;
            });

            services.AddScoped<IWorkContext, WorkContext>();
            services.AddScoped<IUserService, UserService>();
        }

        public static void AddMiddlewares(this WebApplication app)
        {

        }
    }
}
