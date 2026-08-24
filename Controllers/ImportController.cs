using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using PopeShenoudaSeminary.Data;
using PopeShenoudaSeminary.Models;
using System.Text;

namespace PopeShenoudaSeminary.Controllers
{
    public class ImportController : Controller
    {
        private readonly AppDbContext _context;

        public ImportController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ImportGrades(IFormFile file)
        {
            try
            {
                // ==========================================
                // Validate File
                // ==========================================

                if (file == null || file.Length == 0)
                {
                    TempData["AlertType"] = "error";
                    TempData["AlertMessage"] = "لم يتم اختيار ملف.";

                    return RedirectToAction("Index");
                }

                if (!Path.GetExtension(file.FileName)
                    .Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                {
                    TempData["AlertType"] = "error";
                    TempData["AlertMessage"] =
                        "يرجى رفع ملف Excel بصيغة .xlsx";

                    return RedirectToAction("Index");
                }

                ExcelPackage.License.SetNonCommercialOrganization(
                    "Pope Shenouda Seminary");

                int importedCount = 0;

                List<string> errors = new();

                // ==========================================
                // Open Excel
                // ==========================================

                using (var package = new ExcelPackage(file.OpenReadStream()))
                {
                    var sheet = package.Workbook.Worksheets.FirstOrDefault();

                    if (sheet == null || sheet.Dimension == null)
                    {
                        TempData["AlertType"] = "error";
                        TempData["AlertMessage"] =
                            "الملف لا يحتوي على بيانات.";

                        return RedirectToAction("Index");
                    }

                    // ==========================================
                    // Excel Columns
                    //
                    // Column 1 = Student Code
                    // Column 2 = Subject Name
                    // Column 3 = Score
                    // ==========================================

                    for (int row = 2; row <= sheet.Dimension.Rows; row++)
                    {
                        var studentCodeText =
                            sheet.Cells[row, 1].Text.Trim();

                        var subjectName =
                            sheet.Cells[row, 2].Text.Trim();

                        var scoreText =
                            sheet.Cells[row, 3].Text.Trim();

                        // Skip completely empty rows
                        if (string.IsNullOrWhiteSpace(studentCodeText) &&
                            string.IsNullOrWhiteSpace(subjectName) &&
                            string.IsNullOrWhiteSpace(scoreText))
                        {
                            continue;
                        }

                        // ==========================================
                        // Validate Student Code
                        // ==========================================
                        var studentCode = studentCodeText;
                        if (string.IsNullOrWhiteSpace(studentCode))
                        {
                            errors.Add(
                                $"الصف {row}: كود الطالب فارغ.");

                            continue;
                        }

                        // ==========================================
                        // Validate Subject Name
                        // ==========================================

                        if (string.IsNullOrWhiteSpace(subjectName))
                        {
                            errors.Add(
                                $"الصف {row}: اسم المادة فارغ.");

                            continue;
                        }

                        // ==========================================
                        // Validate Score
                        // ==========================================

                        if (!double.TryParse(
                                scoreText,
                                out double score))
                        {
                            errors.Add(
                                $"الصف {row}: الدرجة ({scoreText}) غير صحيحة.");

                            continue;
                        }

                        // ==========================================
                        // Find Student By Code
                        // ==========================================

                        var student = await _context.Users
                            .FirstOrDefaultAsync(x =>
                                x.Code == studentCode);

                        if (student == null)
                        {
                            errors.Add(
                                $"الصف {row}: الطالب بالكود ({studentCode}) غير موجود.");

                            continue;
                        }

                        // ==========================================
                        // Validate Student Grade
                        // ==========================================

                        if (student.GradeId <= 0)
                        {
                            errors.Add(
                                $"الصف {row}: الطالب ({student.FullName}) " +
                                $"ليس له GradeId.");

                            continue;
                        }

                        // ==========================================
                        // Find Subject
                        //
                        // Subject is identified by:
                        //
                        // Subject.Name
                        // +
                        // Student.GradeId
                        // ==========================================

                        var subject = await _context.Subjects
                            .FirstOrDefaultAsync(x =>
                                x.GradeId == student.GradeId &&
                                x.Name.Trim() == subjectName);

                        if (subject == null)
                        {
                            errors.Add(
                                $"الصف {row}: المادة ({subjectName}) " +
                                $"غير موجودة للصف رقم ({student.GradeId}).");

                            continue;
                        }

                        // ==========================================
                        // Validate Score
                        // ==========================================

                        if (score < subject.MinScore ||
                            score > subject.MaxScore)
                        {
                            errors.Add(
                                $"الصف {row}: درجة مادة ({subject.Name}) " +
                                $"يجب أن تكون بين {subject.MinScore} " +
                                $"و {subject.MaxScore}.");

                            continue;
                        }

                        // ==========================================
                        // Find Existing Student Grade
                        //
                        // Student + Subject + Grade
                        // ==========================================

                        var studentGrade =
                            await _context.StudentSubjectGrades
                                .FirstOrDefaultAsync(x =>
                                    x.StudentId == student.Id &&
                                    x.SubjectId == subject.Id &&
                                    x.GradeId == student.GradeId);

                        // ==========================================
                        // Add New Grade
                        // ==========================================

                        // 1. First, verify the student actually has a Grade assigned
                        if (student.GradeId == null)
                        {
                            // Handle this gracefully based on your app's architecture 
                            // (e.g., return a BadRequest, show an error message, or log it)
                            throw new InvalidOperationException("Cannot save a score for a student who is not assigned to a Grade.");
                        }

                        // 2. Proceed safely knowing student.GradeId has a valid value
                        if (studentGrade == null)
                        {
                            _context.StudentSubjectGrades.Add(
                                new StudentSubjectGrade
                                {
                                    StudentId = student.Id,
                                    SubjectId = subject.Id,
                                    GradeId = student.GradeId.Value, // Safely use the actual valid ID
                                    Score = score
                                });
                        }
                        else
                        {
                            // ======================================
                            // Update Existing Grade
                            // ======================================

                            studentGrade.Score = score;
                        }

                        importedCount++;
                    }

                    // ==========================================
                    // Save Changes
                    // ==========================================

                    await _context.SaveChangesAsync();
                }

                // ==========================================
                // Show Result
                // ==========================================

                if (errors.Any())
                {
                    StringBuilder sb = new();

                    sb.AppendLine(
                        $"تم استيراد {importedCount} سجل بنجاح.<br><br>");

                    sb.AppendLine(
                        "<strong>الأخطاء:</strong><br>");

                    foreach (var error in errors)
                    {
                        sb.AppendLine(
                            $"• {error}<br>");
                    }

                    TempData["AlertType"] = "error";
                    TempData["AlertMessage"] = sb.ToString();
                }
                else
                {
                    TempData["AlertType"] = "success";

                    TempData["AlertMessage"] =
                        $"تم استيراد {importedCount} درجة بنجاح.";
                }
            }
            catch (Exception ex)
            {
                TempData["AlertType"] = "error";

                TempData["AlertMessage"] =
                    $"حدث خطأ أثناء استيراد الملف: {ex.Message}";
            }

            return RedirectToAction(
                "AllStudentsGrades",
                "Admin");
        }
    }
}