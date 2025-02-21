using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Moment3MVC.Models
{
    public class Loans
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public Book Book { get; set; } = new Book();
        [Required]
        public DateTime LoanDate { get; set; }
        [Required]
        public DateTime ReturnDate { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
