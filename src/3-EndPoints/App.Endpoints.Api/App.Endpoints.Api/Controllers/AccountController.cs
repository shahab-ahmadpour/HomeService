using App.Domain.Core.DTO.Users.AppUsers;
using App.Domain.Core.Enums;
using App.Domain.Core.Users.Interfaces.IAppService;
using App.Endpoints.Api.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace App.Endpoints.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserAppService _userAppService;
        private readonly Serilog.ILogger _logger;

        public AccountController(
            IUserAppService userAppService,
            Serilog.ILogger logger)
        {
            _userAppService = userAppService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestModel model, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (!model.Role.Equals("Expert", StringComparison.OrdinalIgnoreCase))
                {
                    return BadRequest(new { error = "فقط ثبت نام کارشناسان از طریق API امکان‌پذیر است." });
                }

                var dto = new CreateAppUserDto
                {
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    ProfilePicture = model.ProfilePicture,
                    Role = UserRole.Expert,
                    IsEnabled = true,
                    IsConfirmed = false,
                    AccountBalance = 0
                };

                var result = await _userAppService.RegisterAsync(dto, model.Password, cancellationToken);

                if (result.Succeeded)
                {
                    _logger.Information("Expert with email {Email} registered successfully via API.", model.Email);
                    return Ok(new
                    {
                        Success = true,
                        Message = "ثبت نام با موفقیت انجام شد. حساب کاربری پس از تأیید مدیر فعال خواهد شد."
                    });
                }

                var errors = result.Errors.Select(e => e.Description).ToList();
                return BadRequest(new
                {
                    Success = false,
                    Errors = errors
                });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "خطا در ثبت نام کارشناس با ایمیل {Email}: {Message}", model.Email, ex.Message);
                return StatusCode(500, new { error = "خطای داخلی سرور", details = ex.Message });
            }
        }
    }
}