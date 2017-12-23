using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wafer.Apis.Models
{
    [Table("Menus")]
    public class Menu
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Url { get; set; }
        [Required]
        [MaxLength(100)]
        public string Text { get; set; }
        public bool IsActive { get; set; }
    }
}
