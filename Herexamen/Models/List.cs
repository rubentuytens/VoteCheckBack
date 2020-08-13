using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Herexamen.Models
{
    public class List
    {
        public long ListID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
       
        public long UserID { get; set; }
        public User User { get; set; }
        public List<Item> Items { get; set; }
        public long CategoryID { get; set; }
        public Category Category { get; set; }
        public Boolean Active { get; set; }
    }
}
