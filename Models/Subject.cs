namespace PopeShenoudaSeminary.Models
{
    public class Subject
    {
        public int Id { get; set; }

        public string Name { get; set; }

        // الدرجة العظمى للمادة
        public int MaxScore { get; set; }
        // الدرجة الصغري للمادة

        public int MinScore { get; set; }
        public Grade Grade { get; set; }
        public int GradeId { get; set; }

        public ICollection<Book> Books { get; set; }
             = new List<Book>();
    }
}