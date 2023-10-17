using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using LoginRegistration.Models;

namespace LoginRegistration.Controllers;

public class UserController : Controller
{
    private readonly ILogger<UserController> _logger;
    private MyContext DB;


    public UserController(ILogger<UserController> logger, MyContext context)
    {
        _logger = logger;
        DB = context;
    }
    // -----------------------------INDEX PAGE
    [HttpGet("")]
    public IActionResult Index()
    {
        return View();
    }

    // ----------------------------- CREATE USER
    [HttpPost("users/create")]
    public IActionResult Create(User newUser)
    {
        if (!ModelState.IsValid)
        {
            return View("Index");
        }
        PasswordHasher<User> Hasher = new PasswordHasher<User>();
        newUser.Password = Hasher.HashPassword(newUser, newUser.Password);
        DB.Add(newUser);
        DB.SaveChanges();
        HttpContext.Session.SetInt32("UserId", newUser.UserId);
        return RedirectToAction("Success", "Home");
    }

    // ----------------------------- LOGIN USER
    [HttpPost("users/login")]
    public IActionResult Login(LoginUser userSubmission)
    {
        if (!ModelState.IsValid)
        {
            return View("Index");
        }

        User? userInDb = DB.Users.FirstOrDefault(u => u.Email == userSubmission.LoginEmail);

        if (userInDb == null)
        {
            ModelState.AddModelError("LoginEmail", "Invalid Credentials");
            return View("Index");
        }
        PasswordHasher<LoginUser> hasher = new PasswordHasher<LoginUser>();
        var result = hasher.VerifyHashedPassword(userSubmission, userInDb.Password, userSubmission.LoginPassword);
        if (result == 0)
        {
            ModelState.AddModelError("LoginEmail", "Invalid Credentials");
            return View("Index");
        }
        HttpContext.Session.SetInt32("UserId", userInDb.UserId);
        return RedirectToAction("Success", "Home");
    }


    // ----------------------------- LOGOUT USER
    [HttpPost("users/logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index");
    }





    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}


// using Microsoft.AspNetCore.Mvc.Filters;

// public class SessionCheckAttribute : ActionFilterAttribute
// {
//     public override void OnActionExecuting(ActionExecutingContext context)
//     {
//         int? userId = context.HttpContext.Session.GetInt32("UserId");
//         if(userId == null)
//         {
//             context.Result = new RedirectToActionResult("Index", "User", null);
//         }
//     }
// }