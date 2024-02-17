using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Base.Domain.Common
{
    public class BaseEntities
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [Column(TypeName = "timestamp without time zone")]
        public DateTime? CreatedDate { get; set; }
        [Required]
        public string? CreatedBy { get; set; }
        [Column(TypeName = "timestamp without time zone")]
        public DateTime? LastModifiedDate { get; set; }
        public string? LastModifiedBy { get; set; }
        [Required]
        public bool? Active { get; set; }


    }
}
