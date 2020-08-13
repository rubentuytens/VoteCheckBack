using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Herexamen.Models;
using Microsoft.AspNetCore.Authorization;

namespace Herexamen.Controllers
{
    [Route("api/items")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public ItemsController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Items
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Item>>> GetItem()
        {
            return await _context.Item.ToListAsync();
        }

        // GET: api/Items/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Item>> GetItem(long id)
        {
            var item = await _context.Item.FindAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            return item;
        }

        // PUT: api/Items/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItem(long id, Item item)
        {
            if (id != item.ItemID)
            {
                return BadRequest();
            }

            _context.Entry(item).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemExists(id))
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

        // POST: api/Items
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Item>> PostItem(Item item)
        {
            _context.Item.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetItem", new { id = item.ItemID }, item);
        }
        [HttpPost("new")]
        public async Task<ActionResult<Item>> PostItemsList(Item[] items)
        {
            for (int i =0; i < items.Count(); i++)
            {
                _context.Item.Add(items[i]);
            }
            await _context.SaveChangesAsync();
            return NoContent();
        }
        // DELETE: api/Items/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Item>> DeleteItem(long id)
        {
            var item = await _context.Item.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _context.Item.Remove(item);
            await _context.SaveChangesAsync();

            return item;
        }

        private bool ItemExists(long id)
        {
            return _context.Item.Any(e => e.ItemID == id);
        }
    }
}
