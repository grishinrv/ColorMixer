using ColorMixer.Contracts.Helpers;
using ColorMixer.Storage.Models;
using Microsoft.EntityFrameworkCore;

namespace ColorMixer.Storage.Services
{
    public sealed class SettingsDbContext : DbContext
    {
        public DbSet<SettingsModel> Settings { get; set; }

        public SettingsDbContext(DbContextOptions<SettingsDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SettingsModel>().HasData(
                new SettingsModel { Key = SettingsHelper.DARK_MODE, Value = "false" },
                new SettingsModel { Key = SettingsHelper.USE_OS_THEME, Value = "true" },
                new SettingsModel { Key = SettingsHelper.SELECTED_THEME, Value = "Blue" },
                new SettingsModel { Key = SettingsHelper.SELECTED_UI_CULTURE, Value = null }
            );
        }
    }
}
