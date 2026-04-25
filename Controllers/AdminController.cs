//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using PopeShenoudaSeminary.Data;
//using PopeShenoudaSeminary.Models;

//namespace PopeShenoudaSeminary.Controllers
//{
//    public class AdminController : Controller
//    {
//        private readonly AppDbContext _context;

//        public AdminController(AppDbContext context)
//        {
//            _context = context;
//        }
//        public IActionResult AddStudentGrade()
//        {
//            // ViewBag.Students = _context.Users.Where(u => u.Role.Name == "Student").ToList();
//            ViewBag.Students = _context.Users
//                                .Include(u => u.Role)
//                                .Where(u => u.Role.Name == "Student")
//                                .ToList();
//            ViewBag.Subjects = _context.Subjects.ToList();

//            return View();
//        }
//        public IActionResult Dashboard()
//        {
//            return View();
//        }

//        [HttpPost]
//        public IActionResult AddStudentGrade(StudentSubjectGrade model)
//        {
//            _context.StudentSubjectGrades.Add(model);
//            _context.SaveChanges();

//            return RedirectToAction("AllGrades");
//        }
//        public IActionResult AllStudentsGrades()
//        {
//            if (HttpContext.Session.GetString("Role") != "Admin")
//                return Unauthorized();

//            var grades = _context.StudentGrades
//                .Include(g => g.Student)
//                .Include(g => g.Grade)
//                .ToList();

//            return View(grades);
//        }
//    }
//}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PopeShenoudaSeminary.Data;
using PopeShenoudaSeminary.Models;

namespace PopeShenoudaSeminary.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
                return RedirectToAction("Login", "Account");

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UploadBook(IFormFile file, string title, int gradeId)
        {
            var path = Path.Combine("wwwroot/books", file.FileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var book = new Book
            {
                Title = title,
                FilePath = "/books/" + file.FileName,
                GradeId = gradeId
            };

            _context.Books.Add(book);
            _context.SaveChanges();

            return RedirectToAction("Books");
        }
        public IActionResult AddStudentGrade()
        {
            ViewBag.Students = _context.Users
                .Include(u => u.Role)
                .Where(u => u.Role.Name == "Student")
                .ToList();

            ViewBag.Subjects = _context.Subjects.ToList();

            return View();
        }

        [HttpPost]
        public IActionResult AddStudentGrade(StudentSubjectGrade model)
        {
            _context.StudentSubjectGrades.Add(model);
            _context.SaveChanges();

            return RedirectToAction("AllStudentsGrades");
        }

        public IActionResult AllStudentsGrades()
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
                return Unauthorized();

            var grades = _context.StudentSubjectGrades
                .Include(g => g.Student)
                .Include(g => g.Subject)
                .ToList();

            return View(grades);
        }
    }
}