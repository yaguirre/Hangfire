using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_CRON.Models
{
    public class User
    {
        public long UserID{ get;set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
