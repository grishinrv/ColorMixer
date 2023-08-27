using ColorMixer.Contracts.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ColorMixer.Storage.Models
{
    [Table("Mixings")]
    public sealed class MixingOperationModel
    {
        [Key]
        [Column("Id")]
        public int MixingId { get; set; }
        [Column("RightColorId")]
        [ForeignKey(nameof(RightColor))]
        public int RightColorId { get; set; }
        [Column("LeftColorId")]
        [ForeignKey(nameof(LeftColor))]
        public int LeftColorId { get; set; }
        [Column("Operation")]
        public MixingType Operation { get; set; }
        [Column("SetId")]
        [ForeignKey(nameof(Set))]
        public int SetId { get; set; }
        public ColorNodeModel? RightColor { get; set; }
        public ColorNodeModel? LeftColor{ get; set; }
        public SetModel? Set { get; set; }
    }
}
