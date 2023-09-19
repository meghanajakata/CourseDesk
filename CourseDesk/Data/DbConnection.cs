using Microsoft.EntityFrameworkCore;
using CourseDesk.Models;

namespace CourseDesk.Data
{
    public class DbConnection: DbContext
    {
        public DbConnection(DbContextOptions<DbConnection> options)
            : base(options){ }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);

            //modelBuilder.Entity<Course>()
            //    .HasKey(c => c.Course_id);

            modelBuilder.Entity<Cart>()
                .HasKey(m => m.Id);

            modelBuilder.Entity<CartItem>()
                .HasKey(m => m.Id);

            modelBuilder.Entity<Category>()
                .HasKey(m => m.Id);

            modelBuilder.Entity<CourseStatus>()
                .HasKey(m => m.Id);

            modelBuilder.Entity<Enrollment>()
                .HasKey(m => m.Id);

            modelBuilder.Entity<InstructorPayment>()
                .HasKey(m => m.Id);

            modelBuilder.Entity<Lesson>()
                .HasKey(m => m.Id);

            modelBuilder.Entity<Payment>()
                .HasKey(m => m.Id);

            modelBuilder.Entity<Query>()
                .HasKey(m => m.Id);

            modelBuilder.Entity<Query>()
                .HasOne(q => q.Instructor)
                .WithMany() // or .WithOne() if it's a one-to-one relationship
                .HasForeignKey(q => q.InstructorId)
                .IsRequired(true);

            modelBuilder.Entity<Query>()
                .HasOne(q => q.Student)
                .WithMany() // or .WithOne() if it's a one-to-one relationship
                .HasForeignKey(q => q.StudentId)
                .IsRequired(true);

            modelBuilder.Entity<Sale>()
                .HasKey(m => m.Id);

            //modelBuilder.Entity<Course>()
            //    .Hasc
            base.OnModelCreating(modelBuilder);

        }

        public DbSet<User>Users { get; set; }
        public DbSet<CourseMaterial> CoursesMaterials { get; set;}
        public DbSet<Cart> Cart { get; set; }
        public DbSet<CartItem> CartItem { get; set; }
        public DbSet<Enrollment> Enrollment { get; set; }   
        public DbSet<Lesson> Lesson { get; set; }
        public DbSet<Payment> Payment { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<CourseStatus> CourseStatus { get; set; }
        public DbSet<InstructorPayment> InstructorPayment { get; set; }
        public DbSet<Query> Query { get; set; }
        //public DbSet<Recommendation> Recommendation { get; set; }
        public DbSet<Sale> Sale { get; set; }
    }
}
