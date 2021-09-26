using Application.Authorization;
using Common.Presentation.Controllers;
using Common.Presentation.Models;
using Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace HRM.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : BaseController
    {

        private readonly ILogger<UserController> _logger;
        private readonly IPrincipal principal;
        private readonly UserManager<ApplicationUser> userManager;

        public UserController(ILogger<UserController> logger, IPrincipal principal, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            this.principal = principal;
            this.userManager = userManager;
        }


        [HttpPost("changePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data sent");

            try
            {
                var userId = principal.GetUserId();

                var user = await userManager.FindByIdAsync(userId.ToString());
                if (user == null)
                    return BadRequest("کاربر با آی دی ارسال شده پیدا نشد");

                if (user.IsDeleted)
                    return BadRequest("کاربر با آی دی ارسال شده پیدا نشد");

                var addpass = await userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                if (!addpass.Succeeded)
                    throw new Exception("خطا در تغییر رمز عبور, " + string.Join("\n", addpass.Errors.Select(s => s.Description)));
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                var userId = principal.GetUserId();

                var user = await userManager.FindByIdAsync(userId.ToString());
                if (user == null)
                    return BadRequest("کاربر با آی دی ارسال شده پیدا نشد");
                if (user.IsDeleted)
                    return BadRequest("کاربر با آی دی ارسال شده پیدا نشد");

                return Ok(new
                {
                    user.Id,
                    username = user.UserName,
                    firstName = user.FirstName,
                    lastName = user.LastName,
                    user.ProfileImage,
                    user.PhoneNumber,
                    user.Email,
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

    }
}
