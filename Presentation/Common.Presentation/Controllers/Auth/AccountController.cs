using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Application.Interfaces;
using Application;
using Common.Presentation.Controllers;
using Common.Presentation.Controllers.Auth.Models;

namespace Common.Presentation.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Developer,Administrator")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
    public class AccountController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager = null;
        private readonly RoleManager<ApplicationRole> _roleManager = null;
        private readonly ICommonDbContext dbContext;

        public AccountController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager,
            ICommonDbContext dbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            this.dbContext = dbContext;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var users = await _userManager.Users.Where(x => x.IsDeleted == false)
                .Select(x => new UsersListDto
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    UserName = x.UserName,
                    Email = x.Email,
                    IsLocked = x.LockoutEnabled && x.LockoutEnd != null && x.LockoutEnd >= DateTime.Now,
                }).ToListAsync();

            foreach (var item in users)
            {
                var roles = await _userManager.GetRolesAsync(new ApplicationUser { Id = item.Id });
                item.Roles = roles.ToList();
            }
            return Ok(users);
        }

        [HttpGet("roles/{id}")]
        public IActionResult GetRolesAsync(int id)
        {
            var roles = _userManager.GetRolesAsync(new ApplicationUser { Id = id });
            return Ok(roles);

        }

        /// <summary>
        /// همه کاربران ، کاربران حذف شده و حذف نشده
        /// </summary>
        /// <returns>لیستی از مشخصات کاربران</returns>
        [HttpGet("all")]
        public IActionResult GetAll()
        {
            var users = _userManager.Users.Select(x => new { x.Id, x.FirstName, x.LastName, x.UserName, x.Email });
            return Ok(users);
        }
        /// <summary>
        ///  کاربران حذف شده
        /// </summary>
        /// <returns>لیستی از مشخصات کاربران</returns>
        [HttpGet("deleted")]
        public IActionResult Getdeleted()
        {
            var users = _userManager.Users.Where(x => x.IsDeleted).Select(x => new { x.Id, x.FirstName, x.LastName, x.UserName, x.Email });
            return Ok(users);
        }


        [HttpPost, Route("existUsername")]
        public async Task<IActionResult> CheckUserName([FromBody] CheckUsernameDto model)
        {
            if (await _userManager.FindByNameAsync(model.Username) == null)
                return Ok(false);
            else
                return Ok(true);
        }

        [HttpPost, Route("create")]
        public async Task<IActionResult> Create(CreateAccountDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest("sent model is not valid");
            if (model.Username.Trim().StartsWith("@"))
                return BadRequest($"نام کاربری نمیتواند با '@' شروع شده باشد");

            var user = new ApplicationUser
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.Username,
                Email = model.Email
            };
            try
            {
                if (await _userManager.FindByNameAsync(user.UserName) != null)
                    throw new Exception("نام کاربری تکراری است");

                var resultCreate = await _userManager.CreateAsync(user, model.Password);

                if (resultCreate.Succeeded && model.Roles != null)
                {
                    var avilablerole = await _roleManager.Roles.Where(x => model.Roles.Contains(x.Name)).Select(x => x.Name).ToListAsync();

                    if (avilablerole != null)
                        if (avilablerole.Any())
                        {
                            var resultRole = await _userManager.AddToRolesAsync(user, avilablerole);
                        }
                }
                return Ok(new
                {
                    model.FirstName,
                    model.LastName,
                    UserName = model.Username,
                    model.Email
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost, Route("edit")]
        public async Task<IActionResult> Edit(EditAccountDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest("sent model is not valid");
            if (model.Username.Trim().StartsWith("@"))
                return BadRequest($"نام کاربری نمیتواند با '@' شروع شده باشد");


            try
            {
                var user = await _userManager.FindByIdAsync(model.UserId.ToString());
                if (user == null || user.IsDeleted)
                    throw new Exception("کاربر با این آی دی پیدا نشد");

                if (await dbContext.Users.AnyAsync(s => s.Id != model.UserId && s.UserName == model.Username))
                    throw new Exception("نام کاربری تکراری است");

                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.UserName = model.Username;
                user.Email = model.Email;

                var resultCreate = await _userManager.UpdateAsync(user);

                dbContext.UserRoles.RemoveRange(dbContext.UserRoles.Where(s => s.UserId == user.Id));
                await dbContext.SaveChangesAsync();
                if (resultCreate.Succeeded && model.Roles != null)
                {
                    var avilablerole = await _roleManager.Roles.Where(x => model.Roles.Contains(x.Name)).Select(x => x.Name).ToListAsync();

                    if (avilablerole != null)
                        if (avilablerole.Any())
                        {
                            var resultRole = await _userManager.AddToRolesAsync(user, avilablerole);
                        }
                }
                if (model.Password != null)
                {
                    await ResetPassword(new ResetPasswordDto()
                    {
                        UserId = user.Id.ToString(),
                        NewPassword = model.Password
                    });
                }
                return Ok(new
                {
                    model.FirstName,
                    model.LastName,
                    UserName = model.Username,
                    model.Email
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost, Route("resetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Sent model is not valid");

            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
                return BadRequest("کاربر با آی دی ارسال شده پیدا نشد");
            try
            {
                if (await _userManager.HasPasswordAsync(user))
                {
                    var removepass = await _userManager.RemovePasswordAsync(user);
                    if (!removepass.Succeeded)
                        throw new Exception();
                }
                var addpass = await _userManager.AddPasswordAsync(user, model.NewPassword);
                if (addpass.Succeeded)
                    return Ok();
                throw new Exception();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost, Route("lock")]
        public virtual async Task<IActionResult> Lock([FromBody] LockUserDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Sent model is not valid");

            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
                return BadRequest("کاربر ارسال شده پیدا نشد");
            var result = await _userManager.SetLockoutEnabledAsync(user, true);
            if (result.Succeeded)
            {
                if (model.Day.HasValue)
                {
                    result = await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddDays(model.Day.Value));
                }
                else
                {
                    result = await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
                }
                return Ok();
            }
            return BadRequest("خطا در انجام عملیات رخ داده است");
        }


        [HttpPost("delete/{userId}")]
        public virtual async Task<IActionResult> Delete(int userId)
        {
            try
            {
                var user = await dbContext.Users.Where(s => s.Id == userId).FirstOrDefaultAsync();
                if (user == null)
                    throw new Exception("کاربر پیدا نشد");

                user.IsDeleted = true;
                user.UserName = $"@D_{user.Id}_{user.UserName}";
                user.NormalizedUserName = $"@D_{user.Id}_{user.NormalizedUserName}";
                await dbContext.SaveChangesAsync();
                return Ok(new { Username = user.UserName });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost, Route("unlock")]
        public virtual async Task<IActionResult> Unlock([FromBody] LockUserDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Sent model is not valid");

            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
                return BadRequest("کابر پیدا نشد");
            var result = await _userManager.SetLockoutEnabledAsync(user, false);
            if (result.Succeeded)
            {
                await _userManager.ResetAccessFailedCountAsync(user);
                return Ok();
            }
            return BadRequest("خطا در انجام عملیات");
        }

        [HttpPost("addRole/{id}")]
        public async Task<IActionResult> AddAccessAsync(int id, [FromBody] List<string> Roles)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
                return BadRequest("کابر پیدا نشد");
            var result = await _userManager.AddToRolesAsync(user, Roles);
            if (result == IdentityResult.Success)
                return Ok();
            return BadRequest("خطا در انجام علمیات");
        }

        [HttpPost("deleteRole/{id}")]
        public async Task<IActionResult> DeleteAccessAsync(int id, [FromBody] List<string> Roles)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
                return BadRequest("کاربر پیدا نشد");
            var result = await _userManager.RemoveFromRolesAsync(user, Roles);
            if (result == IdentityResult.Success)
                return Ok();
            return BadRequest("خطا در انجام عملیات");
        }

    }
}