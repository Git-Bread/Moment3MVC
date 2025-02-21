using System.ComponentModel.DataAnnotations;

namespace Moment3MVC.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Title is required")]
        [StringLength(40, ErrorMessage = "Title cannot be longer than 40 characters, abbreviate it")]
        public string Title { get; set; } = string.Empty;
        [Required(ErrorMessage = "Title is required")]
        [StringLength(20, ErrorMessage = "Authors name should not be longer then 20 characters, abbreviate it or remove middle-names")]
        public string Author { get; set; } = string.Empty;
        [Required(ErrorMessage = "Published Date is needed")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime PublishedDate { get; set; } = DateTime.UnixEpoch;
        [Required(ErrorMessage = "Description is needed")]
        public string BookDescription { get; set; } = string.Empty;
    }
}
