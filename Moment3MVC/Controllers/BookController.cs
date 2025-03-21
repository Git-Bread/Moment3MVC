﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moment3MVC.Data;
using Moment3MVC.Models;
using Moment3MVC.ViewModels;

namespace Moment3MVC.Controllers
{
    public class BookController : Controller
    {
        //context "controller" (is that what you call it?) to interact with the database
        private readonly BookDbContext _context;
        //DEBUGGING
        private readonly ILogger<BookController> _logger;

        //Constructor
        public BookController(BookDbContext context, ILogger<BookController> logger)
        {
            _context = context;
            _logger = logger;
        }
        
        //Search remembering
        string searchMode = "Default";
        public string UpdateSearchMode(string mode)
        {
            searchMode = mode;
            return searchMode;
        }

        //Check for expired loans
        private async Task CheckExpiredLoans()
        {
            var currentDate = DateTime.Now;
            var expiredLoans = await _context.Loans
                .Include(l => l.Book)
                .Where(l => l.ReturnDate < currentDate && l.Book != null && l.Book.IsLoaned)
                .ToListAsync();

            if (expiredLoans.Any())
            {
                foreach (var loan in expiredLoans)
                {
                    if (loan.Book != null)
                    {
                        //Set the book as available again
                        loan.Book.IsLoaned = false;
                        _context.Update(loan.Book);
                        _logger.LogInformation($"Returned book: {loan.Book.Title} from {loan.User?.Name}.");
                    }
                }
                await _context.SaveChangesAsync();
            }
        }

        //Index with Search Functionality
        public async Task<IActionResult> Index(string searchString, string searchMode)
        {
            await CheckExpiredLoans();
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

        public async Task<IActionResult> Loan()
        {
            // Make sure to include these using statements at the top of your file:
            // using Microsoft.EntityFrameworkCore;
            
            var loans = await _context.Loans
                .Include(l => l.Book)
                .Include(l => l.User)
                .OrderByDescending(l => l.LoanDate)
                .ToListAsync();
            
            // Debug logging to see what's going on
            foreach (var loan in loans)
            {
                _logger.LogInformation($"Loan {loan.LoanId}: Book = {loan.Book?.Title ?? "null"}, User = {loan.User?.Name ?? "null"}");
            }
            
            return View(loans);
        }

        //Books Create View
        public IActionResult Create()
        {
            return View();
        }

        //Books Create Database Entry
        [HttpPost]
        [ValidateAntiForgeryToken] //Prevents CSRF, good practice
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
        //it seems like this is standard practice, but not sure since why would DELETE exist then?
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                // First find the book without trying to include Loans
                var book = await _context.Books.FindAsync(id);
                    
                if (book == null)
                {
                    return RedirectToAction(nameof(Index));
                }
                
                // Find any associated loans
                var associatedLoans = await _context.Loans
                    .Where(l => l.BookId == id)
                    .ToListAsync();
                
                // Remove any associated loans first
                if (associatedLoans.Any())
                {
                    _context.Loans.RemoveRange(associatedLoans);
                    await _context.SaveChangesAsync();
                }
                
                // Now remove the book
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
                
                TempData["Success"] = "Book successfully deleted.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting book with ID {BookId}", id);
                TempData["Error"] = "Could not delete the book. It may be linked to active loans.";
            }
            
            return RedirectToAction(nameof(Index));
        }

        //Book Update
        public async Task<IActionResult> Update(int? id)
        {
            var book = await _context.Books.FindAsync(id);
            Console.WriteLine(book);
            if (book == null)
            {
                TempData["Error"] = "Book not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        //Book Update Database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, [Bind("Id,Title,Author,PublishedDate, BookDescription")] Book book)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (Exception error)
                {
                    ModelState.AddModelError(error.ToString(), "An error occurred while updating the book.");
                    return View(book);
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

        //Borrow Book
        [HttpGet]
        public IActionResult Borrow(int id)
        {
            //Query the book from the database
            var book = _context.Books.FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                TempData["Error"] = "Book not found.";
                return RedirectToAction("Index");
            }
            
            if (book.IsLoaned)
            {
                TempData["Error"] = "Book is already loaned.";
                return RedirectToAction("Index");
            }
            
            //Create a new loan entry, might be better with a foreign key setup
            var viewModel = new BorrowViewModel
            {
                BookId = book.Id,
                BookTitle = book.Title,
                BookAuthor = book.Author,
                BookPublishedDate = book.PublishedDate,
                BookDescription = book.BookDescription,
                ReturnDate = DateTime.Now.AddDays(3)
            };
    
            
            return View(viewModel);
        }


        //Borrow Book Database Functionality
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Borrow(BorrowViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            var book = await _context.Books.FindAsync(model.BookId);
            if (book == null)
            {
                TempData["Error"] = "Book not found.";
                return RedirectToAction("Index");
            }

            if (book.IsLoaned)
            {
                TempData["Error"] = "Book is already loaned.";
                return RedirectToAction("Index");
            }
            
            //Get the user
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Name == model.UserName);
            if (user == null)
            {
                user = new User { Name = model.UserName };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }
            
            var loan = new Loans
            {
                BookId = model.BookId,
                UserId = user.UserId,
                LoanDate = DateTime.Now,
                ReturnDate = model.ReturnDate,
                Book = book,
                User = user
            };

            try
            {
                //Update book status
                book.IsLoaned = true;
                _context.Update(book);
                
                //Add the loan
                _context.Loans.Add(loan);
                await _context.SaveChangesAsync();
                
                TempData["Success"] = "Book successfully borrowed!";
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error creating loan");
                ModelState.AddModelError("", "An error occurred while processing the loan.");
                return View(model);
            }
        }
    }
}