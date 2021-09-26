using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAuthorizeService
    {
        bool HasAccess<TRequest>(TRequest request, IPrincipal principal);
        /// <summary>
        /// Check scope in user and its role in Database for ther specific operation and key
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="operation"></param>
        /// <param name="key"></param>
        /// <param name="principal"></param>
        /// <returns></returns>
        Task<bool> HasAnyScope(string scope, string operation, string key, IPrincipal principal);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="operation"></param>
        /// <param name="principal"></param>
        /// <returns>returns empty array if it has not the scope and operation access</returns>
        Task<string[]> GetScopeEntities(string scope, string operation, IPrincipal principal);
        bool HasScopeClaim(string scope, IPrincipal principal);
    }
}
