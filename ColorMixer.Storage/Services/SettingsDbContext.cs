using ColorMixer.Storage.Models;
using Microsoft.EntityFrameworkCore;

namespace ColorMixer.Storage.Services
{
    public class SettingsDbContext : DbContext
    {
        public DbSet<SettingsModel> Settings { get; set; }
    }
}
