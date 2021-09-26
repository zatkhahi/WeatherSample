using Application.Interfaces;
using FluentValidation;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Infrastructure
{
    public class RequestAuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
       where TRequest : IRequest<TResponse>
    {
        IPrincipal principal;
        IAuthorizeService authorizeService;

        public RequestAuthorizationBehavior(IPrincipal principal, IAuthorizeService authorizeService)
        {
            this.principal = principal;
            this.authorizeService = authorizeService;
        }

        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            try
            {
                if (!authorizeService.HasAccess(request, principal))
                    throw new Exceptions.UnAuthorizedException("دسترسی مجاز نمی باشد");
            }
            catch (Exceptions.UnAuthorizedException ex)
            {
                throw ex;
            }
            catch (System.Exception ex)
            {
                throw new Exceptions.UnAuthorizedException("دسترسی مجاز نمی باشد", ex);
            }
            return next();
        }
    }
}
