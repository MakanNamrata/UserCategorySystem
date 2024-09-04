using Microsoft.EntityFrameworkCore;
using UserCategorySystem.Models;
#nullable disable

namespace UserCategorySystem.Context;

public class UserCategorySystemContext : DbContext
{
    public UserCategorySystemContext(DbContextOptions<UserCategorySystemContext> options) : base(options)
    { }

    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<SubCategory> SubCategories { get; set; }
    public virtual DbSet<UserCategory> UserCategories { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(x => x.IsAdmin).HasDefaultValueSql("('0')");
        });

        modelBuilder.Entity<SubCategory>(entity => 
        {
            entity.HasOne(x => x.Category)
                    .WithMany(x => x.SubCategories)
                    .HasForeignKey(x => x.CategoryId)
                    .HasConstraintName("FK_SubCategory_Category")
                    .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<UserCategory>(entity => 
        {
            entity.HasOne(x => x.User)
                    .WithMany(x => x.UserCategories)
                    .HasForeignKey(x => x.UserId)
                    .HasConstraintName("FK_UserCategory_User")
                    .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(x => x.Category)
                    .WithMany(x => x.UserCategories)
                    .HasForeignKey(x => x.CategoryId)
                    .HasConstraintName("FK_UserCategory_Category")
                    .OnDelete(DeleteBehavior.Cascade);
        });
    }
}