﻿using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hesabdar.Models;

namespace Hesabdar.Controllers
{
    [Produces("application/json")]
    [Route("api/Dealer")]
    public class DealerController : Controller
    {
        private readonly HesabdarContext _context;

        public DealerController(HesabdarContext context)
        {
            _context = context;
        }

        // GET: api/Dealer
        [HttpGet]
        public IActionResult Dealers([FromQuery] int page = 1, [FromQuery] int perPage = 10, [FromQuery] string sort = "id desc", [FromQuery] string filter = "")
        {
            
            var incomes = _context.Payment.Where(u => u.Paid).Where(u => u.PayerId == 1).GroupBy(u => u.PayeeId).Select(u => new { DealerId = u.Key, Amount = u.Select(i => i.Amount).DefaultIfEmpty(0).Sum()});
            var expenses = _context.Payment.Where(u => u.Paid).Where(u => u.PayeeId == 1).GroupBy(u => u.PayerId).Select(u => new { DealerId = u.Key, Amount = u.Select(i => i.Amount).DefaultIfEmpty(0).Sum()});
            
            var dealers = (
                from d in _context.Dealer
                join i in incomes on d.Id equals i.DealerId into iIncome
                from income in iIncome.DefaultIfEmpty()
                join e in expenses on d.Id equals e.DealerId into iExpenses
                from expense in iExpenses.DefaultIfEmpty()
                where d.Id != 1
                select new Dealer {
                    Address = d.Address,
                    Id = d.Id,
                    Name = d.Name,
                    PhoneNumber = d.PhoneNumber,
                    Timestamp = d.Timestamp,
                    Balance =  (expense!= null ? expense.Amount : 0) - (income != null ? income.Amount : 0)
                }
            ).OrderBy(sort).PageResult(page, perPage);
            return Ok(dealers);
        }

        [HttpGet("Suggest")]
        [HttpGet("Suggest/{text}")]
        public IActionResult GetSuggestedDealers([FromRoute] string text = "")
        {
            text = text.ToLower();
            var dealers = _context.Dealer
                .Where(u => u.Id != 1)
                .Where(u => u.Name.ToLower().Contains(text))
                .Take(10);
            return Ok(dealers);
        }

        // GET: api/Dealer/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDealer([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var incomes = _context.Payment.Where(u => u.Paid).Where(u => u.PayerId == 1).GroupBy(u => u.PayeeId).Select(u => new { DealerId = u.Key, Amount = u.Select(i => i.Amount).DefaultIfEmpty(0).Sum()});
            var expenses = _context.Payment.Where(u => u.Paid).Where(u => u.PayeeId == 1).GroupBy(u => u.PayerId).Select(u => new { DealerId = u.Key, Amount = u.Select(i => i.Amount).DefaultIfEmpty(0).Sum()});
            
            var dealers = (
                from d in _context.Dealer
                join i in incomes on d.Id equals i.DealerId into iIncome
                from income in iIncome.DefaultIfEmpty()
                join e in expenses on d.Id equals e.DealerId into iExpenses
                from expense in iExpenses.DefaultIfEmpty()
                select new Dealer {
                    Address = d.Address,
                    Id = d.Id,
                    Name = d.Name,
                    PhoneNumber = d.PhoneNumber,
                    Timestamp = d.Timestamp,
                    Balance =  (expense!= null ? expense.Amount : 0) - (income != null ? income.Amount : 0)                    
                }
            );
            
            var dealer = await dealers.SingleOrDefaultAsync(m => m.Id == id);

            if (dealer == null)
            {
                return NotFound();
            }

            return Ok(dealer);
        }

        // PUT: api/Dealer/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDealer([FromRoute] int id, [FromBody] Dealer dealer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != dealer.Id)
            {
                return BadRequest();
            }

            _context.Entry(dealer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DealerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Dealer
        [HttpPost]
        public async Task<IActionResult> PostDealer([FromBody] Dealer dealer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Dealer.Add(dealer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDealer", new { id = dealer.Id }, dealer);
        }

        // DELETE: api/Dealer/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDealer([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dealer = await _context.Dealer.SingleOrDefaultAsync(m => m.Id == id);
            if (dealer == null)
            {
                return NotFound();
            }

            _context.Dealer.Remove(dealer);
            await _context.SaveChangesAsync();

            return Ok(dealer);
        }

        private bool DealerExists(int id)
        {
            return _context.Dealer.Any(e => e.Id == id);
        }
    }
}