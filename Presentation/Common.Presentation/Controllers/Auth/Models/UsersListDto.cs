using System.Collections.Generic;

namespace Common.Presentation.Controllers.Auth.Models
{

    /// <summary>
    /// مشخصات کاربران سیستم
    /// </summary>
    /// <returns>لیستی از مشخصات کاربران</returns>
    public class UsersListDto
    {
        public int Id { set; get; }
        public string FirstName { set; get; }
        public string LastName { set; get; }
        public string UserName { set; get; }
        public string Email { set; get; }
        public bool IsLocked { set; get; }
        public IList<string> Roles { set; get; } = new List<string>();
    }


}