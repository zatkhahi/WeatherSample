using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Common.Presentation.Controllers.Auth.Models
{
    public class LockUserDto
    {

        [Required]
        public string UserId { get; set; }
        public int? Day { set; get; }
    }
}
