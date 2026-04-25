using Microsoft.EntityFrameworkCore;
using PopeShenoudaSeminary.Models;
using System.Data;
using System.Diagnostics;

namespace PopeShenoudaSeminary.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<StudentGrade> StudentGrades { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<StudentSubjectGrade> StudentSubjectGrades { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Schedule> Schedules { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId);

            modelBuilder.Entity<StudentGrade>()
                .HasOne(sg => sg.Student)
                .WithMany(u => u.StudentGrades)
                .HasForeignKey(sg => sg.StudentId);

            // --- Fixed: Books ---
            modelBuilder.Entity<Book>()
                .HasOne(b => b.Grade)
                .WithMany()
                .HasForeignKey(b => b.GradeId)
                .OnDelete(DeleteBehavior.NoAction);

            // --- Fixed: Schedules ---
            modelBuilder.Entity<Schedule>()
                .HasOne(s => s.Grade)
                .WithMany()
                .HasForeignKey(s => s.GradeId)
                .OnDelete(DeleteBehavior.NoAction);

            // --- THE NEW FIX: StudentSubjectGrades ---
            //modelBuilder.Entity<StudentSubjectGrade>()
            //    .HasOne(ssg => ssg.Grade) // Assumes the navigation property is named 'Grade'
            //    .WithMany()
            //    .HasForeignKey(ssg => ssg.GradeId)
            //    .OnDelete(DeleteBehavior.NoAction);
        }
    }
}