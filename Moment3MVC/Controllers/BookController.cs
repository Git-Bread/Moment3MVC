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

        //Search remembering
        string searchMode = "Default";
        public string UpdateSearchMode(string mode)
        {
            searchMode = mode;
            return searchMode;
        }

        //Index with Search Functionality
        public async Task<IActionResult> Index(string searchString, string searchMode)
        {
            var books = from b in _context.Books select b;

            if (!string.IsNullOrEmpty(searchString))
            {
                switch (searchMode)
                {
                    case "Title":
                        books = books.Where(s => s.Title.Contains(searchString));
                        break;
                    case "Author":
                        books = books.Where(s => s.Author.Contains(searchString));
                        break;
                    case "PublishedDate":
                        if (int.TryParse(searchString, out int year))
                        {
                            books = books.Where(s => s.PublishedDate.Year == year);
                        }
                        break;
                    default:
                        books = books.Where(s => s.Title.Contains(searchString) || s.Author.Contains(searchString));
                        break;
                }
                UpdateSearchMode(searchMode);
            }

            return View(await books.ToListAsync());
        }

        //Books Create
        public IActionResult Create()
        {
            return View();
        }

        //Books Create Database Entry
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,Title,Author,PublishedDate,BookDescription")] Book book)
        {
            if (ModelState.IsValid)
            {
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        //Delete Books, not a DELETE call.... since post was easier to implement and html forms dont natively support DELETE.
        // it seams like this is standard practice, but not sure since why would DELETE exist then?
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return RedirectToAction(nameof(Index));
            }
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //Book Update
        public async Task<IActionResult> Update(int? id)
        {
            var book = await _context.Books.FindAsync(id);
            Console.WriteLine(book);
            if (book == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        //Book Update Database
        [HttpPost]
        public async Task<IActionResult> Update(int id, [Bind("Id,Title,Author,PublishedDate, BookDescription")] Book book)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (Exception)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        //Book Description
        public async Task<IActionResult> Book(int? id)
        {
            var book = await _context.Books.FindAsync(id);
            Console.WriteLine(book);
            if (book == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

    }
}