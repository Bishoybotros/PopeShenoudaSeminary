namespace PopeShenoudaSeminary.Models
{
    public class Subject
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int GradeId { get; set; }

        public Grade Grade { get; set; }

        public ICollection<Book> Books { get; set; }
    }
}