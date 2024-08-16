using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using LaserCraftHub.Context;
using LaserCraftHub.Models;
using LaserCraftHub.ViewModels;

namespace LaserCraftHub.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private readonly ApplicationContext _context;

    public HomeController(ILogger<HomeController> logger, ApplicationContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet("")]
    public IActionResult Index(string? message)
    {
        ViewBag.Message = message;

        var homeViewModel = new HomePageViewModel()
        {
            User = new User(),
            LoginUser = new LoginUser()
        };
        return View("Index", homeViewModel);
    }


    [HttpPost("register")]
    public IActionResult Register(User newUser)
    {
        if (!ModelState.IsValid)
        {
            var homeViewModel = new HomePageViewModel()
            {
                User = new User(),
                LoginUser = new LoginUser(),
            };
            return View("Index", homeViewModel);
        }
        var hasher = new PasswordHasher<User>();
        newUser.Password = hasher.HashPassword(newUser, newUser.Password);
        _context.Users.Add(newUser);
        _context.SaveChanges();

        HttpContext.Session.SetInt32("userId", newUser.UserId);

        return RedirectToAction("Crafts", "Craft");
    }

    [HttpPost("login")]
    public IActionResult Login(LoginUser loginUser)
    {
        if (!ModelState.IsValid)
        {
            var homeViewModel = new HomePageViewModel()
            {
                User = new User(),
                LoginUser = new LoginUser(),
            };
            return View("Index", homeViewModel);
        }
        var user = _context.Users.SingleOrDefault((user) => user.Email == loginUser.Email);
        if (user is null)
        {
            return RedirectToAction("Index", new { message = "invalid-credentials" });
        }
        var hasher = new PasswordHasher<User>();

        PasswordVerificationResult result = hasher.VerifyHashedPassword(
            user,
            user.Password,
            loginUser.Password
        );

        if (result == 0)
        {
            return RedirectToAction("Index", new { message = "invalid-credentials" });
        }

        HttpContext.Session.SetInt32("userId", user.UserId);

        return RedirectToAction("Crafts", "Craft");
    }

    [HttpGet("logout")]
    public RedirectToActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index");
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
