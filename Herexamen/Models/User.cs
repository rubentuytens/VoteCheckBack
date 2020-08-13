using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Herexamen.Models
{
    public class User
    {
        public long UserID{ get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<List> Lists { get; set; }
        public List<Vote> Votes { get; set; }
        [NotMapped]
        public string Token { get; set; }

    }
}
