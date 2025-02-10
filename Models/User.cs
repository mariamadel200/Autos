using Autos.Migrations;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Autos.Models
{
    public class User:IdentityUser
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z]", ErrorMessage = "No numbers allowed in FirstName")]
        public string Firstname { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z]", ErrorMessage = "No numbers allowed in  LasttName")]
        public string Lastname { get; set; }

        public string? Image { get; set; }
    }
}
