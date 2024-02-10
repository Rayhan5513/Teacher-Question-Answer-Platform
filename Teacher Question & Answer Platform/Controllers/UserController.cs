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
                if (user != null)
                {
                    ModelState.AddModelError("Email", "Email already exist, Please login");
                    return View(model);
                }

                user = new User
                {
                    FirstName = model.FirstName,
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
                        InstituteName = model?.InstituteName ?? "",
                        VersityIdCard = model?.VersityIdCard ?? "",
                        UserId = user.Id
                    };
                    await _userService.InsertUserStudentMappintAsync(mapModel);
                }
                // now login the uesr automatically 
                await SigninAsync(user);
                return RedirectToAction("Index", "Home");
            }
            else ModelState.AddModelError("error", "Please enter all field correctly");
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
            else ModelState.AddModelError("error", "Please enter valid credential");
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
                        return RedirectToAction("Index","Home");
                    }
                }
            }
            ModelState.AddModelError("error", "Please enter valid input filed");
            return View(model);
        }

        public async Task<IActionResult> QuestionDetails(int id)
        {
            var userIdentity = User.Identity as ClaimsIdentity;

            if (userIdentity != null && userIdentity.IsAuthenticated)
            {
                var questionDetails = await _userService.GetQuestionDetailsByIdAsync(id);

                return View(questionDetails);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> CreateAns(int questionId, string answer)
        {
            var userIdentity = User.Identity as ClaimsIdentity;
            var possible = false;
            if (userIdentity != null && userIdentity.IsAuthenticated)
            {
                var email = userIdentity.FindFirst(ClaimTypes.Email)?.Value??"";
                var user = await _userService.GetUserByEmailAsync(email);
                var question = await _userService.GetQuestionByIdAsync(questionId);
                if(question != null && user!=null && question.CreatorId == user.Id)
                {
                    possible = true;
                }
                if (user != null && !user.IsStudent)
                    possible = true;
                if (possible)
                {
                    var ans = new Answer()
                    {
                        AnswererId = user?.Id??0,
                        Comment = answer,
                        QuestionId = questionId,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                    };
                    await _userService.InsertAnswerAsync(ans);
                    return RedirectToAction("QuestionDetails", "User", new {id = questionId});
                }
            }
            ModelState.AddModelError("error", "You don't have permission to add answer for this question");
            return RedirectToAction("QuestionDetails", "User", new { id = questionId });
        }

        public async Task<IActionResult> MyQuestions()
        {
            var userIdentity = User.Identity as ClaimsIdentity;

            if (userIdentity != null && userIdentity.IsAuthenticated)
            {
                var email = userIdentity.FindFirst(ClaimTypes.Email)?.Value??"";
                if(email != null)
                {
                    var user = await _userService.GetUserByEmailAsync(email);
                    if (user != null)
                    {
                        var model = new List<QuestionOverviewModel>();
                        ViewBag.isStudent = false;
                        if (user.IsStudent)
                        {
                            model = await _userService.GetQuestionsForUserAsync(email:email);
                            ViewBag.isStudent = true;
                        }
                        else
                        {
                            model = await _userService.GetQuestionsForUserAsync(teacherId:user.Id);
                        }
                        return View(model);
                    }
                }
            }
            return RedirectToAction("Index","Home");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var userIdentity = User.Identity as ClaimsIdentity;

            if (userIdentity != null && userIdentity.IsAuthenticated)
            {
                var email = userIdentity.FindFirst(ClaimTypes.Email)?.Value ?? "";
                if (email != null)
                {
                    var user = await _userService.GetUserByEmailAsync(email);
                    var question = await _userService.GetQuestionByIdAsync(id);
                    var answers = await _userService.GetQuestionDetailsByIdAsync(id);
                    if(question != null && user !=null && question.CreatorId == user.Id && answers.Answers.Count==0) 
                    {
                        await _userService.DeleteQuestionAsync(question);
                    }
                }
            }
            return RedirectToAction("MyQuestions", "User");
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
