using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PopeShenoudaSeminary.Data;
using PopeShenoudaSeminary.ViewModels;

namespace PopeShenoudaSeminary.Controllers
{
    public class StudentController : Controller
    {
        private readonly AppDbContext _context;

        public StudentController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Books()
        {
            int userId = HttpContext.Session.GetInt32("UserId").Value;

            var student = _context.Users.FirstOrDefault(u => u.Id == userId);

            var books = _context.Books
                .Where(b => b.GradeId == student.GradeId)
                .ToList();

            return View(books);
        }

        public IActionResult Dashboard()
        {
            //int userId = HttpContext.Session.GetInt32("UserId").Value;
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");
            var student = _context.Users.FirstOrDefault(u => u.Id == userId);

            var model = new StudentDashboardViewModel
            {
                Books = _context.Books
                    .Where(b => b.GradeId == student.GradeId)
                    .ToList(),

               

                Grades = _context.StudentSubjectGrades
                    .Include(g => g.Subject)
                    .Where(g => g.StudentId == userId)
                    .ToList(),

                Schedule = _context.Schedules
                    .Include(s => s.Subject)
                    .Where(s => s.GradeId == student.GradeId)
                    .ToList()
            };
            return View(model);
        }
        public IActionResult MyGrades()
        {
            int userId = HttpContext.Session.GetInt32("UserId").Value;

            var grades = _context.StudentSubjectGrades
                .Include(x => x.Subject)
                .Where(x => x.StudentId == userId)
                .ToList();

            return View(grades);
        }
    }
}