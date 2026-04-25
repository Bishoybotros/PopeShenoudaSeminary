using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PopeShenoudaSeminary.Data;
using PopeShenoudaSeminary.Models;

public class GradesController : Controller
{
    private readonly AppDbContext _context;

    public GradesController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var grades = _context.StudentSubjectGrades
            .Include(g => g.Student)
            .Include(g => g.Subject)
            .ToList();

        return View(grades);
    }

    public IActionResult Create()
    {
        ViewBag.Students = _context.Users
            .Where(u => u.Role.Name == "Student")
            .ToList();

        ViewBag.Subjects = _context.Subjects.ToList();

        return View();
    }

    [HttpPost]
    public IActionResult Create(StudentSubjectGrade grade)
    {
        _context.StudentSubjectGrades.Add(grade);
        _context.SaveChanges();

        return RedirectToAction("Index");
    }
}