using System.ComponentModel.DataAnnotations;

namespace Moment3MVC.Models
{
    public class Book
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Author { get; set; } = string.Empty;
        public DateTime PublishedDate { get; set; } = DateTime.UnixEpoch;
    }
}
