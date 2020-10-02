using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WingtipSSO.POCOS;

namespace WingtipSSO.WebApi.Models
{
    public class UserDto
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Telephone { get; set; }
        public string Nationality { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public ICollection<String> Roles { get; set; }
    }
}
