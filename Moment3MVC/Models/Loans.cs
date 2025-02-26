using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Moment3MVC.Models
{
    public class Loans
    {
        [Key]
        public int LoanId { get; set; }
        [Required]
        public int BookId { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        public DateTime LoanDate { get; set; }
        [Required]
        public DateTime ReturnDate { get; set; }
        [ForeignKey("BookId")]
        public virtual Book? Book { get; set; }
    }
}
