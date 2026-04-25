namespace PopeShenoudaSeminary.Models
{
    public class Role
    {
        public int Id { get; set; } // 1 Admin, 2 Doctor, 3 Student
        public string Name { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
