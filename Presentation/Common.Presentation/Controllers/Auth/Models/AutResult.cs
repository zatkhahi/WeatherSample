using System.Collections.Generic;

namespace Common.Presentation.Controllers.Auth.Models
{
    public class AutResult
    {
        public string Token { get; set; }
        public bool Result { get; set; }
        public List<string> Errors { get; set; }
        public string FullName { get; set; }
    }
}
