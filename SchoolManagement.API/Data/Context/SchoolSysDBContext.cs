using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolManagement.API.Data.Seeds;
using SchoolManagement.API.Models;

namespace SchoolManagement.API.Data.Context
{
    public class SchoolSysDBContext : DbContext
    {
        public SchoolSysDBContext(DbContextOptions<SchoolSysDBContext> options) : base(options) { }

        #region Properties

        public DbSet<User> Users { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Grade> Grades { get; set; }

        #endregion

        #region Methods

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region User
            modelBuilder.Entity<User>(u =>
            {
                u.HasKey(u => u.Id);
                u.Property(u => u.Id).ValueGeneratedOnAdd();
                u.Property(u => u.Email).IsRequired();
                u.Property(u => u.Password).IsRequired();
                u.Property(u => u.Role).IsRequired();

                u.HasOne(u => u.Teacher)
                 .WithOne(t => t.User)
                 .HasForeignKey<Teacher>(t => t.UserId)
                 .IsRequired();

                u.HasOne(u => u.Student)
                 .WithOne(s => s.User)
                 .HasForeignKey<Student>(s => s.UserId)
                 .IsRequired();
            });
            #endregion

            #region Teacher 
            modelBuilder.Entity<Teacher>(t =>
            {
                t.HasKey(t => t.Id);
                t.Property(t => t.Id).ValueGeneratedOnAdd();
                t.Property(t => t.Name).IsRequired();
                t.Property(t => t.Surname).IsRequired();
                t.Property(t => t.BirthDate).IsRequired();
                t.Property(t => t.Address).IsRequired();
                t.Property(t => t.MobileNumber).IsRequired();
            });
            #endregion

            #region Subject
            modelBuilder.Entity<Subject>(sub =>
            {
                sub.HasKey(sub => sub.Id);
                sub.Property(sub => sub.Id).ValueGeneratedOnAdd();
                sub.Property(sub => sub.Title).IsRequired();

                sub.HasMany(sub => sub.Teachers)
                       .WithMany(t => t.Subjects)
                       .UsingEntity<Dictionary<string, object>>(
                           "SubjectTeacher",
                           x => x.HasOne<Teacher>().WithMany().HasForeignKey("TeacherId"),
                           x => x.HasOne<Subject>().WithMany().HasForeignKey("SubjectId"),
                           x => x.HasData(
                               new { TeacherId = 1, SubjectId = 3 },
                               new { TeacherId = 2, SubjectId = 2 },
                               new { TeacherId = 3, SubjectId = 1 }
                           )
                       );
            }); 
            #endregion

            #region Class
            modelBuilder.Entity<Class>(c =>
            {
                c.HasKey(c => c.Id);
                c.Property(c => c.Id).ValueGeneratedOnAdd();
                c.Property(c => c.Course).IsRequired();
                c.Property(c => c.Divition).IsRequired();
                c.Property(c => c.Capacity).IsRequired();

                c.HasMany(c => c.Teachers)
                   .WithMany(t => t.Classes)
                   .UsingEntity<Dictionary<string, object>>(
                       "ClassTeacher",
                       x => x.HasOne<Teacher>().WithMany().HasForeignKey("TeacherId"),
                       x => x.HasOne<Class>().WithMany().HasForeignKey("ClassId"),
                       x => x.HasData(
                           new { ClassId = 1, TeacherId = 1 },
                           new { ClassId = 2, TeacherId = 2 },
                           new { ClassId = 3, TeacherId = 3 }
                       )
                   );

                c.HasMany(c => c.Students)
                .WithOne(s => s.Class)
                .HasForeignKey(s => s.ClassId);
            });
            #endregion

            #region Student
            modelBuilder.Entity<Student>(st =>
            {
                st.HasKey(st => st.Id);
                st.Property(st => st.Id).ValueGeneratedOnAdd();
                st.Property(st => st.Name).IsRequired();
                st.Property(st => st.Surname).IsRequired();
                st.Property(st => st.BirthDate).IsRequired();
                st.Property(st => st.Address).IsRequired();
                st.Property(st => st.MobileNumber).IsRequired();

                st.HasMany(st => st.Grades)
                .WithOne(g => g.Student)
                .HasForeignKey(g => g.StudentId);
            });
            #endregion

            #region Attendance
            modelBuilder.Entity<Attendance>(a =>
            {
                a.HasKey(a => a.Id);
                a.Property(a => a.Id).ValueGeneratedOnAdd();
                a.Property(a => a.Date).IsRequired();
                a.Property(a => a.Present).IsRequired();

                a.HasOne(a => a.Student)
                .WithMany(s => s.Attendances)
                .HasForeignKey(a => a.StudentId);

                a.HasOne(a => a.Teacher)
                .WithMany(t => t.Attendances)
                .HasForeignKey(a => a.TeacherId);
            });
            #endregion

            #region Grade
            modelBuilder.Entity<Grade>(g =>
            {
                g.HasKey(g => g.Id);
                g.Property(g => g.Id).ValueGeneratedOnAdd();
                g.Property(g => g.Value).IsRequired();
                g.Property(g => g.Date).IsRequired();

                g.HasOne(g => g.Subject)
                .WithMany(sub => sub.Grades);
            });
            #endregion

            #region Seeds
            modelBuilder.ApplyConfiguration(new UserSeed());
            modelBuilder.ApplyConfiguration(new TeacherSeed());
            modelBuilder.ApplyConfiguration(new SubjectSeed());
            modelBuilder.ApplyConfiguration(new ClassSeed());
            modelBuilder.ApplyConfiguration(new StudentSeed());
            modelBuilder.ApplyConfiguration(new AttendanceSeed());
            modelBuilder.ApplyConfiguration(new GradeSeed());
            #endregion
        }
        #endregion
    }
}