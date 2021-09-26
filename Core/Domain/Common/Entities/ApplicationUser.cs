using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    
    public class ApplicationUser :  IdentityUser<int>
    {
        public string FirstName { set; get; }
        public string LastName { set; get; }
        public bool IsDeleted { get; set; } = false;
        public string ProfileImage { get; set; }

        public virtual ICollection<ApplicationUserClaim> UserClaims { get; set; }
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
        

        public virtual string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
        //public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        //{
        //    var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
        //    userIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, this.UserId));
        //    return userIdentity;
        //}
        public static ApplicationUser System { get; } = new ApplicationUser
        {
            Id = 1,
            UserName = "system",
            Email = "",
            FirstName = "system",
            LastName = "system",
            LockoutEnabled = true,
            LockoutEnd = DateTime.MaxValue,
            PasswordHash = "Cannot Login"
        };
    }
    
}
