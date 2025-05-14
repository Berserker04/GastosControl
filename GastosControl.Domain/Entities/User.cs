using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GastosControl.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public required string Identification { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Login { get; set; }
        public required string Password { get; set; }
        public DateTime BirthDate { get; set; }
        public required string Address { get; set; }
        public required string Email { get; set; }
        public required string Phone { get; set; }
    }
}
