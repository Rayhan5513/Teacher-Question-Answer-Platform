using System.Security.Claims;
using TeacherStudentQAPlatform.Services;

namespace TeacherStudentQAPlatform.Middlewares
{
    public class WorkContextCreatorMiddleware
    {
        private readonly RequestDelegate _next;
        public WorkContextCreatorMiddleware(RequestDelegate next)
        {
            _next = next;
        }


        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                var _workContext = context.RequestServices.GetService<IWorkContext>();
                var email = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value ?? DateTime.UtcNow.Ticks.ToString();

            }
            catch (Exception ex)
            {
                
            }

            await _next(context);
        }
    }


    public static class WorkContextCreatorMiddlewareExtension
    {
        public static IApplicationBuilder UseWorkContextCreatorMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<WorkContextCreatorMiddleware>();
        }
    }
}
