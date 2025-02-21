using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moment3MVC.Data;
using Moment3MVC.Models;

namespace Moment3MVC.Controllers
{
    public class BookController : Controller
    {
        //context "controller" (is that what you call it?) to interact with the database
        private readonly BookDbContext _context;
        public BookController(BookDbContext context)
        {
            _context = context;
        }

        // GET: Index with books list
        public async Task<IActionResult> Index()
        {
            return View(await _context.Books.ToListAsync());
        }

        // GET: Books
        public IActionResult Create()
        {
            return View();
        }

        // POST: Create
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,Title,Author,PublishedDate")] Book book)
        {
            if (ModelState.IsValid)
            {
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }
    }
}