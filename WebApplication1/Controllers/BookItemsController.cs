using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookItemsController : ControllerBase
    {
        private readonly BooksContext _context;

        public BookItemsController(BooksContext context)
        {
            _context = context;
        }

        // GET: api/BookItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookItem>>> GetBookItems()
        {
            return await _context.BookItems.ToListAsync();
        }

        // GET: api/BookItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookItem>> GetBookItem(long id)
        {
            var bookItem = await _context.BookItems.FindAsync(id);

            if (bookItem == null)
            {
                return NotFound();
            }

            return bookItem;
        }

        // PUT: api/BookItems/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBookItem(long id, BookItem bookItem)
        {
            if (id != bookItem.Id)
            {
                return BadRequest();
            }

            if (!BookItemExists(id))
            {
                return NotFound();
            }

            _context.Entry(bookItem).State = EntityState.Modified;

            await _context.SaveChangesAsync();
          
            return Ok(bookItem);
        }

        // POST: api/BookItems
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<BookItem>> PostBookItem(BookItem bookItem)
        {
            if (bookItem.Price < 0)
            {
                return BadRequest("Price value must be greater than 0");
            }

            string year = DateTime.Now.Year.ToString();
            int currentYear = Int32.Parse(year);

            if (bookItem.Year > currentYear)
            {
                return BadRequest("Book year can't overpass current year");
            }
          
            _context.BookItems.Add(bookItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBookItem", new { id = bookItem.Id }, bookItem);
        }
        
        // DELETE: api/BookItems/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<BookItem>> DeleteBookItem(long id)
        {
            var bookItem = await _context.BookItems.FindAsync(id);
            if (bookItem == null)
            {
                return NotFound();
            }

            _context.BookItems.Remove(bookItem);
            await _context.SaveChangesAsync();

            return bookItem;
        }

        private bool BookItemExists(long id)
        {
            return _context.BookItems.Any(e => e.Id == id);
        }
    }
}
