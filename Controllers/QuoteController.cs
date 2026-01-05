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
            {                 return NotFound($"No quotes found for category: {category}");
            }
            return Ok(quote);
        }
    }
}
