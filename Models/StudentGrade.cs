using System.ComponentModel.DataAnnotations.Schema;

namespace PopeShenoudaSeminary.Models
{
    public class StudentGrade
    {
        public int Id { get; set; }

        public int StudentId { get; set; }
        public User Student { get; set; }

        public int GradeId { get; set; }
        public Grade Grade { get; set; }

        public DateTime Year { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal FinalScore { get; set; }
    }
}
