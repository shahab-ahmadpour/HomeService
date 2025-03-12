using App.Domain.Core.DTO.Users.AppUsers;
using App.Domain.Core.Enums;
using App.Domain.Core.Users.Interfaces.IAppService;
using App.Endpoints.Api.Filters;
using App.Endpoints.Api.Models;
using Microsoft.AspNetCore.Mvc;
using System;
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

        public AccountController(IUserAppService userAppService, Serilog.ILogger logger)
        {
            _userAppService = userAppService;
            _logger = logger;
        }

        /// <summary>
        /// ثبت نام کارشناس یا متخصص
        /// </summary>
        /// <param name="model">مدل ثبت نام</param>
        /// <param name="cancellationToken">توکن لغو</param>
        /// <returns>نتیجه ثبت نام</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestModel model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!model.Role.Equals("Expert", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest("فقط ثبت نام کارشناسان از طریق API امکان‌پذیر است.");
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
            _logger.Warning("Failed to register expert {Email} via API. Errors: {Errors}",
                model.Email, string.Join(", ", errors));

            return BadRequest(new
            {
                Success = false,
                Errors = errors
            });
        }
    }
}