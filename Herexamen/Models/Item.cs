using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Herexamen.Models
{
    public class Item
    {
        public long ItemID { get; set; }
     
        public long ListID { get; set; }
        
        public List List { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public List<Vote> Votes { get; set; }
    }
}
