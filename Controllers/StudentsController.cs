using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PopeShenoudaSeminary.Data;
using PopeShenoudaSeminary.Models;
using Microsoft.AspNetCore.Identity;
public class StudentsController : Controller
{
    private readonly AppDbContext _context;

    public StudentsController(AppDbContext context)
    {
        _context = context;
    }

    private void LoadDropdowns(int? roleId = null, int? gradeId = null)
    {
        ViewBag.RoleId = new SelectList(new[]
        {
            new { Id = 1, Name = "أدمن" },
            new { Id = 2, Name = "معلم" },
            new { Id = 3, Name = "طالب" }
        }, "Id", "Name", roleId);

        ViewBag.GradeId = new SelectList(new[]
        {
            new { Id = 1, Name = "الأولى" },
            new { Id = 2, Name = "الثانية" },
            new { Id = 3, Name = "الثالثة" },
            new { Id = 4, Name = "الرابعة" }
        }, "Id", "Name", gradeId);
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
        LoadDropdowns();
        return View();
    }
    [HttpPost]
    public IActionResult Create(User student)
    {
        if (_context.Users.Any(x => x.Email == student.Email))
        {
            ModelState.AddModelError("Email", "البريد الإلكتروني مستخدم بالفعل");
        }

        if (_context.Users.Any(x => x.Code == student.Code))
        {
            ModelState.AddModelError("Code", "الكود مستخدم بالفعل");
        }
        if (string.IsNullOrWhiteSpace(student.PasswordHash))
        {
            ModelState.AddModelError("PasswordHash", "كلمة المرور مطلوبة");
        }

        if (!ModelState.IsValid)
        {
            LoadDropdowns(student.RoleId, student.GradeId);
            return View(student);
        }

        var hasher = new PasswordHasher<string>();

        student.PasswordHash =
            hasher.HashPassword(null, student.PasswordHash);

        _context.Users.Add(student);
        _context.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    // EDIT
    public IActionResult Edit(int id)
    {
        var student = _context.Users.Find(id);

        if (student == null)
            return NotFound();

        LoadDropdowns(student.RoleId, student.GradeId);

        return View(student);
    }


    [HttpPost]
    public IActionResult Edit(User student)
    {
        var existing = _context.Users.Find(student.Id);

        if (existing == null)
            return NotFound();

        existing.FullName = student.FullName;
        existing.Code = student.Code;
        existing.Email = student.Email;
        existing.NationalId = student.NationalId;
        existing.RoleId = student.RoleId;
        existing.GradeId = student.GradeId;

        if (!string.IsNullOrWhiteSpace(student.PasswordHash))
        {
            var hasher = new PasswordHasher<string>();

            existing.PasswordHash =
                hasher.HashPassword(null, student.PasswordHash);
        }

        _context.SaveChanges();

        return RedirectToAction(nameof(Index));
    }
    
    
    //[HttpPost]
    //public IActionResult Edit(User student)
    //{
    //    if (!ModelState.IsValid)
    //    {
    //        LoadDropdowns(student.RoleId, student.GradeId);
    //        return View(student);
    //    }

    //    _context.Users.Update(student);
    //    _context.SaveChanges();

    //    return RedirectToAction(nameof(Index));
    //}

    // DELETE
    public IActionResult Delete(int id)
    {
        var student = _context.Users.Find(id);

        if (student == null)
            return NotFound();

        _context.Users.Remove(student);
        _context.SaveChanges();

        return RedirectToAction(nameof(Index));
    }
}