using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Teacher_Question___Answer_Platform.Models;
using TeacherStudentQAPlatform.Data;
using TeacherStudentQAPlatform.Domains;
using TeacherStudentQAPlatform.Models;
using TeacherStudentQAPlatform.Services;

namespace TeacherStudentQAPlatform.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IWorkContext _workContext;

        public UserController(IUserService userService,
            IWorkContext workContext)
        {
            _userService = userService;
            _workContext = workContext;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userService.GetUserByEmailAsync(model.Email.Trim());
                if(user != null)
                {
                    ModelState.AddModelError("Email", "Email already exist, Please login");
                    return View(model);
                }

                user = new User
                {
                    FirstName = model.FirstName ,
                    LastName = model.LastName,
                    Email = model.Email,
                    CreatedAt = DateTime.Now,
                    IsStudent = model.IsStudent,
                    Password = model.Password, // need hashing TODO : task -2 
                    UpdatedAt = DateTime.Now,
          
                };

                await _userService.InsertUserAsync(user);

                if (model.IsStudent)
                {
                    var mapModel = new UserStudentMapping
                    {
                        InstituteName = model.InstituteName,
                        VersityIdCard = model.VersityIdCard,
                        UserId = user.Id
                    };
                    await _userService.InsertUserStudentMappintAsync(mapModel);
                }
                // now login the uesr automatically 
                await SigninAsync(user);
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

        public IActionResult Login()
        {
            return View();
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userService.GetUserByEmailAsync(model.Email);
                if (user != null)
                {
                    await SigninAsync(user);
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }

        public async Task<IActionResult> CreateQuestion()
        {
            return View(new QuestionModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateQuestion(QuestionModel model)
        {
            if (!ModelState.IsValid) return View();
            var userIdentity = User.Identity as ClaimsIdentity;

            if (userIdentity != null && userIdentity.IsAuthenticated)
            {
                var emailClaim = userIdentity.FindFirst(ClaimTypes.Email);

                if (emailClaim != null)
                {
                    string userEmail = emailClaim.Value;
                    var user = await _userService.GetUserByEmailAsync(userEmail);
                    if(user?.IsStudent??false)
                    {
                        var question = new Question
                        {
                            Title = model.Title,
                            Description = model.Description,
                            CreatedAt = DateTime.UtcNow,
                            CreatorId = user.Id
                        };
                        await _userService.InsertQuestionAsync(question);
                    }
                    return View(questions);
                }
            }
            return View(new QuestionModel());
        }
        private async Task SigninAsync(User model)
        {
            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, model.Email),
                };

            var identity = new ClaimsIdentity(authClaims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            var props = new AuthenticationProperties();

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, props);

        }
    }
}
