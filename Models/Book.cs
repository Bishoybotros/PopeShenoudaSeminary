namespace PopeShenoudaSeminary.Models
{
    public class Book
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string FilePath { get; set; }

        public int SubjectId { get; set; }

        public Subject Subject { get; set; }

        public int GradeId { get; set; }

        public Grade Grade { get; set; }


    }
}