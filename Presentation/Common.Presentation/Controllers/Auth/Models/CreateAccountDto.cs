using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Common.Presentation.Controllers.Auth.Models
{
    public class CreateAccountDto
    {
        [MaxLength(30)]
        [Required]
        public string FirstName { set; get; }
        [MaxLength(30)]
        [Required]
        public string LastName { set; get; }
        [MaxLength(30)]
        [Required]
        public string Username { set; get; }
        [EmailAddress]
        //[Required]
        public string Email { set; get; }

        public List<string> Roles { set; get; }

        [MinLength(6)]
        [MaxLength(50)]
        [Required]
        public string Password { set; get; }

        [MinLength(6)]
        [MaxLength(50)]
        [Compare(nameof(Password))]
        [Required]
        public string ConfirmPassword { set; get; }
    }

    public class EditAccountDto
    {
        [Required]
        public int UserId { get; set; }

        [MaxLength(30)]
        [Required]
        public string FirstName { set; get; }
        [MaxLength(30)]
        [Required]
        public string LastName { set; get; }
        [MaxLength(30)]
        [Required]
        public string Username { set; get; }
        [EmailAddress]
        //[Required]
        public string Email { set; get; }

        public List<string> Roles { set; get; }

        [MinLength(6)]
        [MaxLength(50)]
        public string Password { set; get; }

        [MinLength(6)]
        [MaxLength(50)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { set; get; }


    }
}
