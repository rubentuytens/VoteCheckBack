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
    [Route("api/votes")]
    [ApiController]
    public class VotesController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public VotesController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Votes
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vote>>> GetVote()
        {
            return await _context.Vote.ToListAsync();
        }

        // GET: api/Votes/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Vote>> GetVote(long id)
        {
            var vote = await _context.Vote.FindAsync(id);

            if (vote == null)
            {
                return NotFound();
            }

            return vote;
        }

        // PUT: api/Votes/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVote(long id, Vote vote)
        {
            if (id != vote.VoteID)
            {
                return BadRequest();
            }

            _context.Entry(vote).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VoteExists(id))
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

        // POST: api/Votes
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Vote>> PostVote(Vote vote)
        {
            _context.Vote.Add(vote);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVote", new { id = vote.VoteID }, vote);
        }
        [Authorize]
        [HttpGet("lists/{userID}")]
        public async Task<ActionResult<IEnumerable<List>>> GetListsVotes(long userID)
        {
            var votes = await _context.Vote.Where(x => x.UserID == userID).Distinct().ToListAsync();
            List<List> lists = new List<List>();
            foreach (Vote l in votes)
            {
                var item =  _context.Item.Find(l.ItemID);
                var list = _context.List.Find(item.ListID);
                lists.Add(list);
            }

           
            foreach (List l in lists)
            {
                _context.Entry(l).Collection(a => a.Items).Load();

            }
            foreach (List l in lists)
            {
                foreach (Item i in l.Items)
                {
                    // var count = _context.Vote.Where(s => s.ItemID == i.ItemID).Count();
                    _context.Entry(i).Collection(a => a.Votes).Load();
                }
            }
            List<List> test = new List<List>();
            foreach(List l in lists)
            {
                if (!test.Exists(x => x.ListID == l.ListID))
                {
                    test.Add(l);
                }
            }
            return test;
        }

        // DELETE: api/Votes/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Vote>> DeleteVote(long id)
        {
            var vote = await _context.Vote.FindAsync(id);
            if (vote == null)
            {
                return NotFound();
            }

            _context.Vote.Remove(vote);
            await _context.SaveChangesAsync();

            return vote;
        }

        private bool VoteExists(long id)
        {
            return _context.Vote.Any(e => e.VoteID == id);
        }
    }
}
