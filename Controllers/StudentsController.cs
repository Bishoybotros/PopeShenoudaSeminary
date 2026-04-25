using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PopeShenoudaSeminary.Data;
using PopeShenoudaSeminary.Models;

public class StudentsController : Controller
{
    private readonly AppDbContext _context;

    public StudentsController(AppDbContext context)
    {
        _context = context;
    }

    // VIEW ALL
    public IActionResult Index()
    {
        var students = _context.Users
            .Include(u => u.Role)
            .Include(u => u.Grade)
            .Where(u => u.Role.Name == "Student")
            .ToList();

        return View(students);
    }

    // CREATE
    public IActionResult Create()
    {
        ViewBag.Grades = _context.Grades.ToList();
        return View();
    }

    [HttpPost]
    public IActionResult Create(User student)
    {
        _context.Users.Add(student);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    // EDIT
    public IActionResult Edit(int id)
    {
        var student = _context.Users.Find(id);
        ViewBag.Grades = _context.Grades.ToList();

        return View(student);
    }

    [HttpPost]
    public IActionResult Edit(User student)
    {
        _context.Users.Update(student);
        _context.SaveChanges();

        return RedirectToAction("Index");
    }

    // DELETE
    public IActionResult Delete(int id)
    {
        var student = _context.Users.Find(id);

        _context.Users.Remove(student);
        _context.SaveChanges();

        return RedirectToAction("Index");
    }
}