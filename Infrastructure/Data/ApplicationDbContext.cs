using Microsoft.EntityFrameworkCore;
using SmartCoins.Core.Entities;

namespace SmartCoins.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<ExpenseCategory> ExpenseCategories { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<SavingsGoal> SavingsGoals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure relationships

            // User email must be unique
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Expense relationships
            modelBuilder.Entity<Expense>()
                .HasOne(e => e.Category)
                .WithMany(c => c.Expenses)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Expense>()
             .HasOne(e => e.User)
             .WithMany(u => u.Expenses)
             .HasForeignKey(e => e.UserId)
             .OnDelete(DeleteBehavior.Restrict);

            // Many-to-many relationship between Expense and Tag
            modelBuilder.Entity<Expense>()
                .HasMany(e => e.Tags)
                .WithMany(t => t.Expenses)
                .UsingEntity(j => j.ToTable("ExpenseTags"));

            // ExpenseCategory relationships
            modelBuilder.Entity<ExpenseCategory>()
                .HasOne(c => c.User)
                .WithMany(u => u.Categories)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Tag relationships
            modelBuilder.Entity<Tag>()
                .HasOne(t => t.User)
                .WithMany(u => u.Tags)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Budget relationships
            modelBuilder.Entity<Budget>()
                .HasOne(b => b.User)
                .WithMany(u => u.Budgets)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Budget>()
                .HasOne(b => b.Category)
                .WithMany(c => c.Budgets)
                .HasForeignKey(b => b.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            // SavingsGoal relationships
            modelBuilder.Entity<SavingsGoal>()
                .HasOne(s => s.User)
                .WithMany(u => u.SavingsGoals)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Expense>()
                .Property(e => e.Amount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Budget>()
                .Property(b => b.Amount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<SavingsGoal>()
                .Property(s => s.TargetAmount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<SavingsGoal>()
                .Property(s => s.CurrentAmount)
                .HasColumnType("decimal(18,2)");
        }
    }
}