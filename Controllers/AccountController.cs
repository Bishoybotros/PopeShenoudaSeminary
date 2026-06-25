using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PopeShenoudaSeminary.Data;
using PopeShenoudaSeminary.Models;
using PopeShenoudaSeminary.ViewModels;

namespace PopeShenoudaSeminary.Controllers

{
    public class AccountController:Controller
    {
        private readonly AppDbContext _context;
        public AccountController(AppDbContext context)
        { 
        _context = context;
        
        }
       
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            model.Email = model.Email?.Trim().ToLower();

            var user = _context.Users
                .Include(u => u.Role)
                .FirstOrDefault(u => u.Email.ToLower() == model.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid email or password");
                return View(model);
            }

            if (user.Role == null)
            {
                ModelState.AddModelError("", "Account configuration error");
                return View(model);
            }

            var hasher = new PasswordHasher<string>();

            var result = hasher.VerifyHashedPassword(
                null,
                user.PasswordHash,
                model.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError("", "Invalid email or password");
                return View(model);
            }

            if (result == PasswordVerificationResult.SuccessRehashNeeded)
            {
                user.PasswordHash = hasher.HashPassword(null, model.Password);
                _context.SaveChanges();
            }

            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("Role", user.Role.Name);

            return user.Role.Name switch
            {
                "Admin" => RedirectToAction("Dashboard", "Admin"),
                "Doctor" => RedirectToAction("Dashboard", "Doctor"),
                "Student" => RedirectToAction("Dashboard", "Student"),
                _ => RedirectToAction("Index", "Home")
            };
        }
    }
}

