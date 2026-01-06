using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Controllers.Models;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/quotes/[controller]")]
    public class QuoteController : ControllerBase
    {
        private readonly QuoteDbContext _db;

        public QuoteController(QuoteDbContext db)
        {
            _db = db;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetQuotes()
        {
            var quotes = await _db.Quotes.ToListAsync();

            if (quotes == null || !quotes.Any())
            {
                return NotFound("No quotes available.");
            }
            return Ok(quotes);
        }

        [HttpGet("category/{category}")]
        public async Task<IActionResult> GetQuoteByCategory(string category)
        {
            var quote = await _db.Quotes.
                Where(c => c.Category.Equals(category))
                .ToListAsync();

            if (quote == null || !quote.Any())
            { return NotFound($"No quotes found for category: {category}");
            }
            return Ok(quote);
        }

        [HttpGet("id/{id}")]
        public async Task<IActionResult> GetQuoteById(int id)
        {
            var quote = await _db.Quotes.FindAsync(id);

            if (quote == null)
            {
                return NotFound($"Quote with ID {id} not found.");
            }

            return Ok(quote);
        }

        [HttpGet("random")]
        public async Task<IActionResult> GetRandomQuote()
        {
            var count = await _db.Quotes.CountAsync();

            if (count == 0)
            {
                return NotFound("No quotes available.");
            }

            Random random = new Random();
            int randomIndex = random.Next(0, count);

            var randomQuote = await _db.Quotes.Skip(randomIndex)
                .Take(1)
                .FirstOrDefaultAsync();
            return Ok(randomQuote);
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateQuote([FromBody] Quote Quote)
        {
            if (Quote == null)
            {
                return BadRequest("Quote cannot be null.");
            }

            await _db.Quotes.AddAsync(Quote);
                        await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetQuoteById), new { id = Quote.Id },
                Quote);
        }

        public async Task<IActionResult> DeleteQuote(int id)
        {
            var quote = await _db.Quotes.FindAsync(id);

            if (quote == null)
            {
                return NotFound($"Quote with ID {id} not found.");
            }

            _db.Quotes.Remove(quote);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
