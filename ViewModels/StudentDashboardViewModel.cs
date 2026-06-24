using PopeShenoudaSeminary.Models;

namespace PopeShenoudaSeminary.ViewModels
{
    public class StudentDashboardViewModel
    {
        public User Student { get; set; } = new();
        public List<Book> Books { get; set; }=new();

        //public List<Lecture> Lectures { get; set; }

        public List<StudentSubjectGrade> Grades { get; set; } = new();

        public List<Schedule> Schedule { get; set; } = new();
        public List<User> users { get; set; } = new();
        public List<Subject> Subjects { get; set; } = new();


    }
}