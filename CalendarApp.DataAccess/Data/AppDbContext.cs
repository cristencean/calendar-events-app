using CalendarApp.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace CalendarApp.DataAccess.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) {}

        public DbSet<CalendarEventModel> CalendarEvents => Set<CalendarEventModel>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CalendarEventModel>(entity =>
            {
                entity.ToTable("CalendarEvents");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(250);
                entity.Property(e => e.StartDate).IsRequired();
                entity.Property(e => e.EndDate).IsRequired();
                entity.Property(e => e.Description).HasMaxLength(2000);
            });
        }
    }
}