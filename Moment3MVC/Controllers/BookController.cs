﻿using Microsoft.AspNetCore.Mvc;
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

        // GET: Books Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Books Create Database Entry
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

        // POST: Delete Books, not a DELETE call.... since post was easier to implement and html forms dont natively support DELETE.
        //  it seams like this is standard practice, but not sure since why would DELETE exist then?
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

        // GET: Book Update
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

        // POST: Book Update Database
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
    }
}