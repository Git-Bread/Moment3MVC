using System.ComponentModel.DataAnnotations;

namespace Moment3MVC.ViewModels
{
    public class BorrowViewModel
    {
        public int BookId { get; set; }
        
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters")]
        [Display(Name = "Your Name")]
        public string UserName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Return date is required")]
        [Display(Name = "Return Date")]
        public DateTime ReturnDate { get; set; }
        
        // Book information for display
        public string? BookTitle { get; set; }
        public string? BookAuthor { get; set; }
        public DateTime? BookPublishedDate { get; set; }
        public string? BookDescription { get; set; }
    }
}