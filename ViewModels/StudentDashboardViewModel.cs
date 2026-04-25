using PopeShenoudaSeminary.Models;

namespace PopeShenoudaSeminary.ViewModels
{
    public class StudentDashboardViewModel
    {
        public List<Book> Books { get; set; }

        //public List<Lecture> Lectures { get; set; }

        public List<StudentSubjectGrade> Grades { get; set; }

        public List<Schedule> Schedule { get; set; }
    }
}