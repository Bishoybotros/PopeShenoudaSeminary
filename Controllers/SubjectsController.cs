using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PopeShenoudaSeminary.Data;
using PopeShenoudaSeminary.Models;

public class SubjectsController : Controller
{
    private readonly AppDbContext _context;

    public SubjectsController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var subjects = _context.Subjects
            .Include(s => s.Grade)
            .ToList();

        return View(subjects);
    }

    public IActionResult Create()
    {
        ViewBag.Grades = _context.Grades.ToList();
        return View();
    }

    [HttpPost]
    public IActionResult Create(Subject subject)
    {
        _context.Subjects.Add(subject);
        _context.SaveChanges();

        return RedirectToAction("Index");
    }

    public IActionResult Delete(int id)
    {
        var subject = _context.Subjects.Find(id);

        _context.Subjects.Remove(subject);
        _context.SaveChanges();

        return RedirectToAction("Index");
    }
}