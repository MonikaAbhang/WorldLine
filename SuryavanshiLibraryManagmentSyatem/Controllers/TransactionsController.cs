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
    public class TransactionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TransactionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Transactions
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Transactions.Include(t => t.Book).Include(t => t.Customer);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Transactions/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions
                .Include(t => t.Book)
                .Include(t => t.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // GET: Transactions/Create
        public IActionResult Create()
        {
            ViewData["BookId"] = new SelectList(_context.Book, "ISBN", "Title");
            ViewData["CustomerId"] = new SelectList(_context.Customer, "Id", "Name");
            var transactions = new Transaction();
            transactions.DateOfIssue = DateTime.Now;
            return View(transactions);
        }

        // POST: Transactions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BookId,CustomerId,DateOfIssue,DateOfReturn")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                _context.Add(transaction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookId"] = new SelectList(_context.Book, "ISBN", "Title", transaction.BookId);
            ViewData["CustomerId"] = new SelectList(_context.Customer, "Id", "Name", transaction.CustomerId);
            return View(transaction);
        }

        // GET: Transactions/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }
            ViewData["BookId"] = new SelectList(_context.Book, "ISBN", "ISBN", transaction.BookId);
            ViewData["CustomerId"] = new SelectList(_context.Customer, "Id", "Id", transaction.CustomerId);
            return View(transaction);
        }

        // POST: Transactions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BookId,CustomerId,DateOfIssue,DateOfReturn")] Transaction transaction)
        {
            if (id != transaction.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transaction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransactionExists(transaction.Id))
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
            ViewData["BookId"] = new SelectList(_context.Book, "ISBN", "ISBN", transaction.BookId);
            ViewData["CustomerId"] = new SelectList(_context.Customer, "Id", "Id", transaction.CustomerId);
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions
                .Include(t => t.Book)
                .Include(t => t.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        private bool TransactionExists(int id)
        {
            return _context.Transactions.Any(e => e.Id == id);
        }
        //public IActionResult Search(string SearchTransaction, string search)
        //{
        //    if (SearchTransaction == "BookId")
        //        return View(_context.Transactions.Where(result => result.BookId == search || search == null).ToList());
        //    return View();
        //}
        //public async Task<IActionResult> IssueBookAsync()
        //{
        //    ViewData["BookId"] = new SelectList(_context.Book, "ISBN", "Title");
        //    ViewData["CustomerId"] = new SelectList(_context.Customer, "Id", "Id");

        //    var transaction = new Transaction();
        //    transaction.DateOfIssue = DateTime.Now;

        //    return View(transaction);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> IssueBookAsync(string bookId, int customerId, DateTime dateOfIssue)
        //{
        //    var transaction = await _context.Transactions
        //       .Where(t => t.BookId == bookId && t.CustomerId == customerId).FirstOrDefaultAsync();

        //    transaction.DateOfIssue = dateOfIssue;
        //    _context.Update(transaction);

        //    var book = await _context.Book.FindAsync(transaction.BookId);
        //    book.IssuedStatus = true;
        //    _context.Update(book);
        //    await _context.SaveChangesAsync();

        //    //var expectedReturnDate = transaction.DateOfIssue.AddDays(7);

        //    ViewData["BookId"] = new SelectList(_context.Book, "ISBN", "Title", transaction.BookId);
        //    ViewData["CustomerId"] = new SelectList(_context.Customer, "Id", "Name", transaction.CustomerId);
        //    return RedirectToAction(nameof(Index));

        //}
        public async Task<IActionResult> ReturnBookAsync()
        {
            ViewData["BookId"] = new SelectList(_context.Book, "ISBN", "Title");
            ViewData["CustomerId"] = new SelectList(_context.Customer, "Id", "Id");

            var transactions = new Transaction();
            transactions.DateOfReturn = DateTime.Now;

            return View(transactions);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReturnBookAsync(string bookId, int customerId, DateTime dateOfReturn)
        {
            var transaction = await _context.Transactions
                .Where(t => t.BookId == bookId && t.CustomerId == customerId).FirstOrDefaultAsync();

            transaction.DateOfReturn = dateOfReturn;
            _context.Update(transaction);

            var book = await _context.Book.FindAsync(transaction.BookId);
            book.IssuedStatus = false;
            _context.Update(book);
            await _context.SaveChangesAsync();

            // var expectedReturnDate = transaction.DateOfIssue.AddDays(7);

            ViewData["BookId"] = new SelectList(_context.Book, "ISBN", "Title", transaction.BookId);
            ViewData["CustomerId"] = new SelectList(_context.Customer, "Id", "Name", transaction.CustomerId);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> LateBookAsync()
        {
            var transactionQuery =
                from t in _context.Transactions
                join b in _context.Book on t.BookId equals b.ISBN
                join c in _context.Customer on t.CustomerId equals c.Id
                join a in _context.Author on b.AuthorId equals a.Id
                select new LateBook(c.Name, a.Name, b.Title, b.ISBN, t.DateOfIssue);

            //select new LateTransactionInfo(c.Name, a.Name, b.Title, b.ISBN,
            // (DateTime.Now - t.DateOfIssue).TotalDays * 100);

            var transactions = await transactionQuery.ToListAsync();
            var transactionsWithFine = transactions.Where(t => (DateTime.Now - t.DateOfIssue).TotalDays > 7);

            return View(transactionsWithFine);
        }
    }
}

