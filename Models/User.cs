using System.Data;
using System.Diagnostics;

namespace PopeShenoudaSeminary.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Code { get; set; } // الكود الموجود في الاكسل

        public string FullName { get; set; } // username

        public string NationalId { get; set; } // الرقم القومي
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }

        public int? GradeId { get; set; }   // current grade = code
        public Grade Grade { get; set; }

        public ICollection<StudentGrade> StudentGrades { get; set; }
    }
}
