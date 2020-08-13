using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Herexamen.Models
{
    public class Vote
    {
        public long VoteID { get; set; }
    
        public long ItemID { get; set; }
        public Item Item { get; set; }


        public long UserID { get; set; }
        public User User { get; set; }

    }
}
