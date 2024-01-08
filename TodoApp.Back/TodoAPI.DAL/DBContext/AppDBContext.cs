using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoAPI.DAL.Entities;

namespace TodoAPI.DAL.DBContext
{
    public class AppDBContext : IdentityDbContext
    {
        public DbSet<Collection> Collections { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Goal> Goals { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<CategoryGoal> CategoriesGoals { get; set; }

        public AppDBContext(DbContextOptions<AppDBContext> options, bool isTest = false) : base(options)
        {
            if (isTest)
                Database.EnsureCreated();
            else
                Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Account>(e =>
            {
                e.HasIndex(x => x.UserName).IsUnique();
                e.HasIndex(x => x.PhoneNumber).IsUnique();
                e.HasIndex(x => x.Email).IsUnique();
            });

            builder.Entity<Category>(e =>
            {
                e.HasKey(x => x.Id);
                e.HasIndex(x => new { x.ColorTitle, x.ColorHex }).IsUnique();
                e.Property(x => x.ColorTitle).HasColumnType("varchar").HasMaxLength(58);
                e.Property(x => x.ColorHex).HasColumnType("varchar").HasMaxLength(10);
                e.HasOne(x => x.Account).WithMany(x => x.Categories).HasForeignKey(x => x.AccountId);
            });

            builder.Entity<Collection>(e =>
            {
                e.HasKey(x => x.Id);
                e.HasIndex(x => new { x.Title, x.AccountId }).IsUnique();
                e.HasOne(x => x.Account).WithMany(x => x.Collections).HasForeignKey(x => x.AccountId).OnDelete(deleteBehavior: DeleteBehavior.Cascade);
            });

            builder.Entity<Goal>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Title).HasColumnType("varchar").HasMaxLength(58);
                e.Property(x => x.Description).HasColumnType("varchar").HasMaxLength(256);
                e.HasOne(x => x.Collection).WithMany(y => y.Goals).HasForeignKey(x => x.CollectionId).OnDelete(deleteBehavior: DeleteBehavior.Cascade);
            });

            builder.Entity<Attachment>(e =>
            {
                e.HasKey(x => x.Id);
                e.HasIndex(x => new { x.Filename, x.Fullpath }).IsUnique();
                e.HasOne(x => x.Goal).WithMany(x => x.Attachments).HasForeignKey(x => x.GoalId).OnDelete(deleteBehavior: DeleteBehavior.Cascade);
            });

            builder.Entity<CategoryGoal>(e =>
            {
                e.HasKey(x => new { x.GoalId, x.CategoryId });
                e.HasOne(x => x.Goal).WithMany(x => x.GoalCategories).HasForeignKey(x => x.GoalId).OnDelete(deleteBehavior: DeleteBehavior.Restrict);
                e.HasOne(x => x.Category).WithMany(x => x.CategoryGoals).HasForeignKey(x => x.CategoryId).OnDelete(deleteBehavior: DeleteBehavior.Cascade);
            });

            base.OnModelCreating(builder);
        }
    }
}
