using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Common.Presentation.Models
{
    public class ChangePasswordDto
    {
        [MinLength(6)]
        [MaxLength(50)]
        [Required]
        public string OldPassword { get; set; }

        [MinLength(6)]
        [MaxLength(50)]
        [Required]
        public string NewPassword { get; set; }
    }
}
