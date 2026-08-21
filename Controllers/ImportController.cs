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
                if (file == null || file.Length == 0)
                {
                    TempData["AlertType"] = "error";
                    TempData["AlertMessage"] = "لم يتم اختيار ملف.";
                    return RedirectToAction("Index");
                }

                if (!Path.GetExtension(file.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                {
                    TempData["AlertType"] = "error";
                    TempData["AlertMessage"] = "يرجى رفع ملف Excel بصيغة .xlsx";
                    return RedirectToAction("Index");
                }

                ExcelPackage.License.SetNonCommercialOrganization("Pope Shenouda Seminary");

                int importedCount = 0;
                List<string> errors = new();

                using (var package = new ExcelPackage(file.OpenReadStream()))
                {
                    var sheet = package.Workbook.Worksheets.FirstOrDefault();

                    if (sheet == null)
                    {
                        TempData["AlertType"] = "error";
                        TempData["AlertMessage"] = "الملف لا يحتوي على بيانات.";
                        return RedirectToAction("Index");
                    }

                    for (int row = 2; row <= sheet.Dimension.Rows; row++)
                    {
                        var studentIdText = sheet.Cells[row, 1].Text.Trim();
                        var subjectIdText = sheet.Cells[row, 2].Text.Trim();
                        var scoreText = sheet.Cells[row, 3].Text.Trim();

                        if (string.IsNullOrWhiteSpace(studentIdText))
                            continue;

                        if (!int.TryParse(studentIdText, out int studentId) ||
                            !int.TryParse(subjectIdText, out int subjectId) ||
                            !int.TryParse(scoreText, out int score))
                        {
                            errors.Add($"الصف {row}: بيانات غير صحيحة.");
                            continue;
                        }

                        var student = await _context.Users
                            .FirstOrDefaultAsync(x => x.Id == studentId);

                        if (student == null)
                        {
                            errors.Add($"الصف {row}: الطالب رقم {studentId} غير موجود.");
                            continue;
                        }

                        var subject = await _context.Subjects
                            .FirstOrDefaultAsync(x => x.Id == subjectId);

                        if (subject == null)
                        {
                            errors.Add($"الصف {row}: المادة رقم {subjectId} غير موجودة.");
                            continue;
                        }

                        if (score < 0 || score > subject.MaxScore)
                        {
                            errors.Add($"الصف {row}: درجة مادة ({subject.Name}) يجب أن تكون بين 0 و {subject.MaxScore}.");
                            continue;
                        }

                        var studentGrade = await _context.StudentSubjectGrades
    .FirstOrDefaultAsync(x =>
        x.StudentId == studentId &&
        x.SubjectId == subjectId);

                        if (studentGrade == null)
                        {
                            // إضافة درجة جديدة
                            _context.StudentSubjectGrades.Add(new StudentSubjectGrade
                            {
                                StudentId = studentId,
                                SubjectId = subjectId,
                                Score = score
                            });
                        }
                        else
                        {
                            // تحديث الدرجة الموجودة
                            studentGrade.Score = score;
                        }

                        importedCount++;
                    }

                    await _context.SaveChangesAsync();
                }

                if (errors.Any())
                {
                    StringBuilder sb = new();

                    sb.AppendLine($"تم استيراد {importedCount} سجل بنجاح.<br><br>");
                    sb.AppendLine("<strong>الأخطاء:</strong><br>");

                    foreach (var error in errors)
                        sb.AppendLine($"• {error}<br>");

                    TempData["AlertType"] = "error";
                    TempData["AlertMessage"] = sb.ToString();
                }
                else
                {
                    TempData["AlertType"] = "success";
                    TempData["AlertMessage"] = $"تم استيراد {importedCount} درجة بنجاح.";
                }
            }
            catch (Exception ex)
            {
                TempData["AlertType"] = "error";
                TempData["AlertMessage"] = ex.Message;
            }

            return RedirectToAction("AllStudentsGrades", "Admin");
        }
    }
}