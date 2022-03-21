using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SuryavanshiLibraryManagmentSyatem.Data;
using SuryavanshiLibraryManagmentSyatem.Models;

namespace SuryavanshiLibraryManagmentSyatem.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BooksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Books
        public async Task<IActionResult> Index()
        {
            return View(await _context.Book.ToListAsync());
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .FirstOrDefaultAsync(m => m.ISBN == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ISBN,Title,PublisherId,AuthorId,IssueStatus,IsDeleted")] Book book)
        {
            if (ModelState.IsValid)
            {
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // POST: Books/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ISBN,Title,PublisherId,AuthorId,IssueStatus,IsDeleted")] Book book)
        {
            if (id != book.ISBN)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.ISBN))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var book = await _context.Book
                .FirstOrDefaultAsync(m => m.ISBN == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var book = await _context.Book.FindAsync(id);
            _context.Book.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool BookExists(string id)
        {
            return _context.Book.Any(e => e.ISBN == id);
        }
        public async Task<IActionResult> AvailableBookAsync()
        {
            List<Book> books = await _context.Book.Where(b => b.IssuedStatus == false).ToListAsync();
            return View(books);
            //var books = _context.Book.Where(e => e.IssuedStatus == false).Include(e => e.Author).Include(e => e.Publisher);
            //return View("AvailableBook", await books.ToListAsync());
        }
        public async Task<IActionResult> IssueBookAsync()
        {
            List<Book> books = await _context.Book.Where(b => b.IssuedStatus == true).ToListAsync();
            return View(books);
            //var books = _context.Book.Where(e => e.IssuedStatus == false).Include(e => e.Author).Include(e => e.Publisher);
            //return View("AvailableBook", await books.ToListAsync());
        }
        public IActionResult Search(string searchBook, string search)
        {
            if (searchBook == "ISBN")
                return View(_context.Book.Where(result => result.ISBN == search || search == null).ToList());
            else
                return View(_context.Book.Where(result => result.Title.StartsWith(search) || search == null).ToList());
        }
        //public IActionResult IssueBooks()
        //{
        //    ViewData["BookId"] = new SelectList(_context.Book, "ISBN", "Title");
        //    ViewData["CustomerId"] = new SelectList(_context.Customer, "Id", "Name");

        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> IssueBooks(string bookId, int customerId)
        //{
        //    var transaction = await _context.Transactions
        //       .Where(t => t.BookId == bookId && t.CustomerId == customerId).FirstOrDefaultAsync();

        //    var book = await _context.Book.FindAsync(transaction.BookId);
        //    book.IssuedStatus = true;
        //    _context.Update(book);
        //    await _context.SaveChangesAsync();

        //    ViewData["BookId"] = new SelectList(_context.Book, "ISBN", "Title", transaction.BookId);
        //    ViewData["CustomerId"] = new SelectList(_context.Customer, "Id", "Name", transaction.CustomerId);
        //    //  transactions.DateOfReturn = transactions.DateOfIssue.AddDays(7);
        //    return RedirectToAction(nameof(Index));
        //}
    }
}

