using ColorMixer.Storage.Models;
using Microsoft.EntityFrameworkCore;

namespace ColorMixer.Storage.Services
{
    public sealed class AppDbContext : DbContext
    {
        public DbSet<ColorNodeModel> Colors { get; set; }
        public DbSet<SetModel> Sets { get; set; }
        public DbSet<MixingOperationModel> Operations { get; set; }

        public AppDbContext(DbContextOptions<SettingsDbContext> options) : base(options)
        {
        }
    }
}
