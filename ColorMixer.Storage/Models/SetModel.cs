using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ColorMixer.Storage.Models
{
    [Table("MixingSets")]
    public class SetModel
    {
        [Key]
        [Column("Id")]
        public int SetId { get; set; }
        [Column("Name")]
        public string? SetName { get; set; }

        public ICollection<MixingOperationModel>? Operations { get; set; }
    }
}
