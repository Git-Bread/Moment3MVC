using System.ComponentModel.DataAnnotations;

namespace Moment3MVC.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Phone]
        [StringLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        // Navigation property for loans
        public List<Loans> Loans { get; set; } = new List<Loans>();
    }
}