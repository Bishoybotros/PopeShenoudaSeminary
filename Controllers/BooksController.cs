using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PopeShenoudaSeminary.Data;
using PopeShenoudaSeminary.Models;

namespace PopeShenoudaSeminary.Controllers
{
    public class BooksController : Controller
    {
        private readonly AppDbContext _context;

        public BooksController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var books = _context.Books
                .Include(b => b.Grade)
                .ToList();

            return View(books);
        }

        public IActionResult Create()
        {
            ViewBag.Grades = _context.Grades.ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(IFormFile file, Book book)
        {
            var path = Path.Combine("wwwroot/books", file.FileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            book.FilePath = "/books/" + file.FileName;

            _context.Books.Add(book);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
