using System.ComponentModel.DataAnnotations;

namespace Moment3MVC.Models
{
    public class Loans
    {
        [Key]
        public int LoanId { get; set; }
        
        [Required(ErrorMessage = "Book ID is required")]
        public int BookId { get; set; }
        
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters")]
        public string Name { get; set; } = string.Empty;
        
        public DateTime LoanDate { get; set; }
        
        [Required(ErrorMessage = "Return date is required")]
        [Display(Name = "Return Date")]
        public DateTime ReturnDate { get; set; }
        
        public Book? Book { get; set; }
    }
}
