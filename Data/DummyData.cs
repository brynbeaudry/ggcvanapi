using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ggcvanapi.Models;
using ggcvanapi.Data;

namespace ggcvanapi.Data
{
    public class DummyData
    {
        public static void Initialize(ApplicationDbContext db)
        {
            if (!db.Events.Any())            {
                //seed, call this function from ADBC
            }
        }
    }
}

