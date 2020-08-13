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
    [Route("api/lists")]
    [ApiController]
    public class ListsController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public ListsController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Lists
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<List>>> GetList()
        {
            return await _context.List.ToListAsync();
        }
        [Authorize]
        [HttpGet("home/{userID}")]
        public async Task<ActionResult<IEnumerable<List>>> GetLastLists(long userID)
        {

            var lists = _context.List.OrderByDescending(l => l.ListID)
                .Where(l => (l.UserID != userID) && (DateTime.Now >= l.StartDate && DateTime.Now < l.EndDate) && (l.Active == true))
                .Take(5)
                .ToList();




        //    var lists2 = lists.ToListAsync();
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

            var votes = await _context.Vote.Where(x => x.UserID == userID).Distinct().ToListAsync();
            List<List> lists2 = new List<List>();
            foreach (Vote l in votes)
            {
                var item = _context.Item.Find(l.ItemID);
                var list = _context.List.Find(item.ListID);
                lists2.Add(list);
            }
            lists2.Select(x => x.ListID).Distinct();
            foreach (List l in lists2)
            {
                
                if (lists.Exists(x => x.ListID == l.ListID))
                {
                    lists.Remove(l);
                }
                lists.Remove(l);
            }
            return lists;
        }
        [Authorize]
        [HttpGet("filter/{str}/{searchCategory}")]
        public async Task<ActionResult<IEnumerable<List>>> GetFilteredLists(string str, long searchCategory)
        {
            if (searchCategory ==1)
            {
                var lists = await _context.List.Where(x => (x.Category.name.ToLower().Contains(str.ToLower())) && (DateTime.Now >= x.StartDate && DateTime.Now < x.EndDate) && (x.Active == true)).ToListAsync();
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
                return lists;
            } else if (searchCategory ==2)
            {
                var lists = await _context.List.Where(x => (x.Name.ToLower().Contains(str.ToLower())) && (DateTime.Now >= x.StartDate && DateTime.Now < x.EndDate) && (x.Active == true)).ToListAsync();
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
                return lists;
            } else if (searchCategory ==3)
            {
                var lists = await _context.List.Where(x =>  (x.User.UserName.ToLower().Contains(str.ToLower())) && (DateTime.Now >= x.StartDate && DateTime.Now < x.EndDate) && (x.Active == true)).ToListAsync();
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
                return lists;
            } else
            {
                return NoContent();
            }
           
        }

        // GET: api/Lists/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<List>> GetList(long id)
        {
            var list = await _context.List.FindAsync(id);

            if (list == null)
            {
                return NotFound();
            }

            return list;
        }
        [HttpGet("user/{userID}")]
        public async Task<ActionResult<IEnumerable<List>>> GetListsFromUser(long userID)
        {
            var lists = await _context.List.Where(x => x.UserID == userID).ToListAsync();
       
            foreach (List l in lists)
            {
                _context.Entry(l).Collection(a => a.Items).Load();
                
            }
            foreach (List l in lists)
            {
                foreach( Item i in l.Items)
                {
                    // var count = _context.Vote.Where(s => s.ItemID == i.ItemID).Count();
                    _context.Entry(i).Collection(a => a.Votes).Load();
                }
            }

            return lists;
        }
        [Authorize]
        [HttpGet("voted/{userID}")]
        public async Task<ActionResult<IEnumerable<List>>> GetVotedListsUser(long userID)
        {
            var lists = await _context.List.Where(x => x.UserID != userID).ToListAsync();

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

            return lists;
        }


        // PUT: api/Lists/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutList(long id, List list)
        {
            if (id != list.ListID)
            {
                return BadRequest();
            }

            _context.Entry(list).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ListExists(id))
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

        // POST: api/Lists
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<List>> PostList(List list)
        {
            _context.List.Add(list);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetList", new { id = list.ListID }, list);
        }



        // DELETE: api/Lists/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<List>> DeleteList(long id)
        {
            var list = await _context.List.FindAsync(id);
            if (list == null)
            {
                return NotFound();
            }

            _context.List.Remove(list);
            await _context.SaveChangesAsync();

            return list;
        }

        private bool ListExists(long id)
        {
            return _context.List.Any(e => e.ListID == id);
        }
    }
}
