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

    // GET: Grades
    public IActionResult Index()
    {
        var grades = _context.StudentSubjectGrades
            .Include(g => g.Student)
            .Include(g => g.Subject)
            .ToList();

        return View(grades);
    }


    // GET: Create
    public IActionResult Create()
    {
        LoadDropdowns();

        return View();
    }


    // POST: Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(StudentSubjectGrade grade)
    {
        if (!ModelState.IsValid)
        {
            LoadDropdowns();
            return View(grade);
        }

        _context.StudentSubjectGrades.Add(grade);
        _context.SaveChanges();

        TempData["Success"] = "Grade added successfully.";

        return RedirectToAction(nameof(Index));
    }


    // GET: Edit
    public IActionResult EditStudentGrade(int id)
    {
        var grade = _context.StudentSubjectGrades
            .FirstOrDefault(x => x.Id == id);


        if (grade == null)
            return NotFound();


        ViewBag.Students = _context.Users
            .Where(x => x.Role.Name == "Student")
            .ToList();


        ViewBag.Subjects = _context.Subjects.ToList();


        return View(grade);
    }


    // POST: Edit
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EditStudentGrade(StudentSubjectGrade model)
    {

        if (!ModelState.IsValid)
        {
            ViewBag.Students = _context.Users
                .Where(x => x.Role.Name == "Student")
                .ToList();

            ViewBag.Subjects = _context.Subjects.ToList();


            return View(model);
        }


        var grade = _context.StudentSubjectGrades
            .FirstOrDefault(x => x.Id == model.Id);


        if (grade == null)
            return NotFound();



        grade.StudentId = model.StudentId;
        grade.SubjectId = model.SubjectId;
        grade.Score = model.Score;


        _context.SaveChanges();


        TempData["Success"] = "تم تعديل الدرجة بنجاح";


        return RedirectToAction(nameof(Index));
    }


    // GET: Delete
    public IActionResult Delete(int id)
    {
        var grade = _context.StudentSubjectGrades
            .Include(g => g.Student)
            .Include(g => g.Subject)
            .FirstOrDefault(g => g.Id == id);


        if (grade == null)
            return NotFound();


        return View(grade);
    }



    // POST: Delete
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var grade = _context.StudentSubjectGrades
            .FirstOrDefault(g => g.Id == id);


        if (grade == null)
            return NotFound();


        _context.StudentSubjectGrades.Remove(grade);
        _context.SaveChanges();


        TempData["Success"] = "Grade deleted successfully.";

        return RedirectToAction(nameof(Index));
    }



    private void LoadDropdowns()
    {
        ViewBag.Students = _context.Users
            .Where(u => u.Role.Name == "Student")
            .ToList();


        ViewBag.Subjects = _context.Subjects
            .ToList();
    }
}