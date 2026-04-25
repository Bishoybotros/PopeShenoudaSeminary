using PopeShenoudaSeminary.Models;

public class Schedule
{
    public int Id { get; set; }

    public string Day { get; set; }

    public string Time { get; set; }

    public int SubjectId { get; set; }

    public Subject Subject { get; set; }

    public int GradeId { get; set; }

    public Grade Grade { get; set; }
}