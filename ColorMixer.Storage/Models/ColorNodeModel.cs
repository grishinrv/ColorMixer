using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ColorMixer.Storage.Models
{
    [Table("ColorNodes")]
    public sealed class ColorNodeModel
    {
        [Key]
        public int Id { get; set; }
        [Column("Red")]
        public byte Red { get; set; }
        [Column("Green")]
        public byte Green { get; set; }
        [Column("Blue")]
        public int Blue { get; set; }
    }
}
