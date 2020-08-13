using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Herexamen.Models
{
    public class DBInitializer
    {
        public static void Initialize(DatabaseContext context)
        {
            context.Database.EnsureCreated();

            if (context.Users.Any())
            {
                return;
            }
        }
    }
}
