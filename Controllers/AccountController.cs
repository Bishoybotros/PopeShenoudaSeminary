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
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = _context.Users
                .Include(u => u.Role)
                .FirstOrDefault(u => u.Email == model.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "Email not found");
                return View(model);
            }

            var hasher = new Microsoft.AspNetCore.Identity.PasswordHasher<string>();

            var result = hasher.VerifyHashedPassword(
                null,
                user.PasswordHash,
                model.Password
            );

            if (result == Microsoft.AspNetCore.Identity.PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError("", "Password incorrect");
                return View(model);
            }

            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("Role", user.Role.Name);

            if (user.Role.Name == "Admin")
                return RedirectToAction("Dashboard", "Admin");

            if (user.Role.Name == "Doctor")
                return RedirectToAction("Dashboard", "Doctor");

            if (user.Role.Name == "Student")
                return RedirectToAction("Dashboard", "Student");

            return View();
        }
    }
}

