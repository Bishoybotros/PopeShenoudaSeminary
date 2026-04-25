namespace PopeShenoudaSeminary.Models
{
    public class Grade
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<User> Students { get; set; }
    }
}
