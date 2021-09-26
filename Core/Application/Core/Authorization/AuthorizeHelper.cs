using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Application.Authorization
{
    public static class CommonAuthorizeHelper
    {
        public static int GetUserId(this IPrincipal principal)
        {
            var id = (principal as ClaimsPrincipal).FindFirst(ClaimTypes.NameIdentifier);
            if (id == null)
                return 0;
            //throw new Exception("توکن صحیح نیست");
            if (!int.TryParse(id.Value, out int uid))
                throw new Exception($"Invalid principal. NameIdentifier {id.Value} is not a User ID");
            return uid;
        }
        public static void StampCreateEntity<TEntity>(this TEntity e, IPrincipal principal) where TEntity : AuditEntity
        {
            e.CreatedAt = DateTime.UtcNow;
            e.CreatedById = principal.GetUserId();
        }
        public static void StampUpdateEntity<TEntity>(this TEntity e, IPrincipal principal) where TEntity : AuditEntity
        {
            e.UpdatedAt = DateTime.UtcNow;
            e.UpdatedById = principal.GetUserId();
        }        
        public static bool IsAdministrator(this IPrincipal principal)
        {
            return principal.IsInRole(ApplicationRole.Administrator.Name);
        }
        public static IPrincipal GetSystemPrincipal()
        {
            return new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
                new Claim(ClaimTypes.NameIdentifier, ApplicationUser.System.Id.ToString())
            }));
        }
        //public async static Task<IPrincipal> CreatePrincipalByUsername(string username, Interfaces.IDMSDbContext db)
        //{
        //    var user = await db.Users.Include(u => u.UserClaims)
        //        .Include(u => u.UserRoles)
        //        .ThenInclude(ur => ur.Role)
        //        .Where(s => s.UserName == username).FirstOrDefaultAsync();
        //    if (user == null)
        //        return null;

        //    var ident = new ClaimsIdentity(new Claim[] {
        //        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        //    });
        //    foreach (var role in user.UserRoles)
        //        ident.AddClaim(new Claim(ClaimTypes.Role, role.Role.Name));

        //    return new ClaimsPrincipal(ident);
        //}
    }
}
