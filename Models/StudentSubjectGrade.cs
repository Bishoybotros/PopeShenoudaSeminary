namespace PopeShenoudaSeminary.Models
{
    public class StudentSubjectGrade
    {
        public int Id { get; set; }

        public int StudentId { get; set; }
        public User Student { get; set; }

        public int SubjectId { get; set; }
        public Subject Subject { get; set; }
        //public Grade Grade { get; set; }
        //  public int GradeId { get; set; }
        public double Score { get; set; }

        public int Year { get; set; }
    }
}