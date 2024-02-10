using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using TeacherStudentQAPlatform.Models;
using TeacherStudentQAPlatform.Services;

namespace TeacherStudentQAPlatform.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService _userService;

        public HomeController(ILogger<HomeController> logger,
            IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var userIdentity = User.Identity as ClaimsIdentity;

            if (userIdentity != null && userIdentity.IsAuthenticated)
            {
                var emailClaim = userIdentity.FindFirst(ClaimTypes.Email);

                if (emailClaim != null)
                {
                    var questions = await _userService.GetQuestionsForUserAsync();
                    return View(questions);
                }
            }
            return RedirectToAction("login", "user");
        }

        public IActionResult Privacy()
        {
            return View();
        }
       
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}