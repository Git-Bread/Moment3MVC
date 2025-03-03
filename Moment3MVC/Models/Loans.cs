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
        public int UserId { get; set; }
        
        public DateTime LoanDate { get; set; }
        
        [Required]
        public DateTime ReturnDate { get; set; }

        // Navigation properties - can be used when needed but not required
        [ForeignKey("UserId")]
        public User? User { get; set; }

        [ForeignKey("BookId")]
        public Book? Book { get; set; }
    }
}