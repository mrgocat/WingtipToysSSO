using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WingtipSSO.WebApi.Models
{
    public class UserCreateDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Telephone { get; set; }
        public string Nationality { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}
