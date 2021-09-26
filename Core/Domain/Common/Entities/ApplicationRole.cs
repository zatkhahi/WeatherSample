using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class ApplicationRole : IdentityRole<int>
    {
        public string Title { get; set; }
        public bool IsBuiltIn { get; set; } = false;

        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
        // public virtual ICollection<RoleScope> Scopes { get; set; }        
        public static ApplicationRole System { get; } = new ApplicationRole()
        {
            Id = 1,
            Name = "System",
            NormalizedName = "SYSTEM",
            ConcurrencyStamp = "4fd748b1-0216-400b-a669-746178a73b2a",
            Title = "سیستم",
            IsBuiltIn = true
        };

        public static ApplicationRole Administrator { get; } = new ApplicationRole()
        {
            Id = 2,
            Name = BuiltInRoles.Administrator,
            NormalizedName = BuiltInRoles.Administrator.ToUpper(),
            ConcurrencyStamp = "71a91ce5-b29f-4cc4-a57c-f69419e231a5",
            Title = "مدیر کل سامانه",
            IsBuiltIn = true
        };

        public static ApplicationRole User { get; } = new ApplicationRole()
        {
            Id = 3,
            Name = "User",
            NormalizedName = "USER",
            ConcurrencyStamp = "1A3F494A-A8A1-4378-B33F-C79C6ED93789",
            Title = "کاربر",
            IsBuiltIn = true
        };

        public static ApplicationRole AuthenticatedUser { get; } = new ApplicationRole()
        {
            Id = 4,
            Name = "AuthenticatedUser",
            NormalizedName = "AUTHENTICATEDUSER",
            ConcurrencyStamp = "DC4F31D0-0DBB-4682-A1BC-2627A1F2D09F",
            Title = "کاربر اعتبارسنجی شده",
            IsBuiltIn = true
        };
        public static ApplicationRole AccountManager { get; } = new ApplicationRole()
        {
            Id = 5,
            Name = "AccountManager",
            NormalizedName = "ACCOUNTMANAGER",
            ConcurrencyStamp = "d8255282-8f55-470f-a2c2-a853d554eaae",
            Title = "مدیر دسترسی و حساب ها",
            IsBuiltIn = true
        };

       
    }

    public static class BuiltInRoles
    {
        public const string Administrator = "Administrator";        
    }
}
