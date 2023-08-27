using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ColorMixer.Storage.Models
{
    [Table("Settings")]
    public class SettingsModel
    {
        [Key]
        [Column("Key")]
#pragma warning disable CS8618
        public string Key { get; set; }
#pragma warning restore CS8618
        [Column("Value")]
        public string? Value { get; set; }
    }
}
