using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
// using DMS.Domain.Entities;
using System.Security.Claims;
using Application.Infrastructure;
using Application.Interfaces;
using System.Threading;
using Common.Presentation.Models;
//using System.Web.Http;
// using System.IdentityModel.Tokens.Jwt;

namespace Common.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public abstract class BaseController : Controller
    {
        private IMediator _mediator;

        protected IMediator Mediator => _mediator ?? (_mediator = HttpContext.RequestServices.GetService<IMediator>());
        //protected IMediator Mediator { get; set; }

        protected int GetUserId() => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

        public BaseController()
        {
        }

        async protected Task<IActionResult> HandleRequest<T>(IRequest<T> command, IServiceScope serviceScope = null)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid Model Data Sent");

            try
            {

                IMediator mediator = serviceScope != null ? serviceScope.ServiceProvider.GetService<IMediator>() : Mediator;
                var result = await mediator.Send(command);
                if (result is Unit)
                {
                    return NoContent();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequest(ex));
            }
        }

    }
}
