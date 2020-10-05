using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WingtipSSO.WebApi.Models
{
    public class UserKeyValueDto
    {
        public static readonly string[] AccecptedKeys = new string[] { "Email", "PasswordHash", "FirstName", "LastName", "Telephone", "Nationality", "DateOfBirth" };
        
        [Required]
        public string key { get; set; }
        [Required]
        public string value { get; set; }
    }
}
