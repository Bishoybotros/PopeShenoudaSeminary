using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using PopeShenoudaSeminary.Data;
using PopeShenoudaSeminary.Models;

namespace PopeShenoudaSeminary.Controllers
{
    public class ImportController : Controller
    {
        private readonly AppDbContext _context;

        public ImportController(AppDbContext context)
        {
            _context = context;
        }

        // 1. You need a GET method to actually display the upload page!
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // 2. The POST method that handles the file
        [HttpPost]
        public IActionResult ImportGrades(IFormFile file)
        {
            // Fix 1: Stop the app from crashing if no file was selected
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file was uploaded.");
            }

            // Fix 2: Prevent the EPPlus License crash
            ExcelPackage.License.SetNonCommercialOrganization("Pope Shenouda Seminary");

            using (var package = new ExcelPackage(file.OpenReadStream()))
            {
                var sheet = package.Workbook.Worksheets[0];

                // Assuming Row 1 is Headers, so we start at Row 2
                for (int row = 2; row <= sheet.Dimension.Rows; row++)
                {
                    var studentIdText = sheet.Cells[row, 1].Text;
                    var subjectIdText = sheet.Cells[row, 2].Text;
                    var scoreText = sheet.Cells[row, 3].Text;

                    // Fix 3: Stop the app from crashing if it hits an empty row
                    if (string.IsNullOrWhiteSpace(studentIdText))
                    {
                        continue;
                    }

                    var grade = new StudentSubjectGrade
                    {
                        StudentId = int.Parse(studentIdText),
                        SubjectId = int.Parse(subjectIdText),
                        Score = int.Parse(scoreText)
                    };

                    _context.StudentSubjectGrades.Add(grade);
                }

                _context.SaveChanges();
            }

            // This will now perfectly redirect to your GradesController!
            return RedirectToAction("Index", "Grades");
        }
    }
}