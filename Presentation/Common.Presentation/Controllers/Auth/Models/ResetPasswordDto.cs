using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Common.Presentation.Controllers.Auth.Models
{
    public class ResetPasswordDto
    {
        [Required]
        public string UserId { set; get; }

        [MinLength(6)]
        [MaxLength(50)]
        [Required]
        public string NewPassword { set; get; }
    }
}
