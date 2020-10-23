using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace devfestcertapi.Models
{
    public class Response
    {
        public string TicketNumber { get; set; }
        public bool IsValid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
