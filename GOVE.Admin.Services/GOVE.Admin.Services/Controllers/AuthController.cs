using AutoMapper;
using GOVE.Infrastructure.Queries;
using GOVE.Models.Requests;
using GOVE.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using static GOVE.Models.Constants.Constants;
using System.Security.Claims;
using System.Text;
using static GOVE.Models.Responses.Response;
using GOVE.Models.Constants;
using GOVE.Infrastructure.Services.Identity_Services;
using GOVE.Models.Entities;

namespace GOVE.Admin.Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IConfiguration configuration, IMediator mediator, IMapper mapper, ILogger<AuthController> logger) : BaseController(mediator, logger, mapper)
    {
        private readonly IConfiguration _configuration = configuration;

        [HttpPost]
        [Route("GetLoginDetails")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(byte[]), StatusCodes.Status200OK, Web.ContentType.Json)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest, Web.ContentType.Json)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status500InternalServerError, Web.ContentType.Json)]
        public async Task<IActionResult> GetUserById(Models.Requests.LoginRequest loginRequest)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(loginRequest.UserName, nameof(loginRequest.UserName));
                ArgumentNullException.ThrowIfNull(loginRequest.Password, nameof(loginRequest.Password));
                var query = new GetLoginQuery.Query(loginRequest.UserName, loginRequest.Password);
                var user = await GoveMediator.Send(query);
                if (user == null)
                    return new BadRequestObjectResult(new Models.Responses.MessageResponse
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Error = new ErrorResponse
                        {
                            Message = Models.Constants.Constants.Messages.INVALID_USER,
                        }
                    });
                var userToken = await IdentityServer4Client.LoginAsync(_configuration[Constants.IdentityServerConfigurationKey]!, loginRequest.UserName, loginRequest.Password);
                user.SessionExpireDate = (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) + userToken.ExpiresIn;
                return Ok(new GoveResponse
                {
                    Status = Status.Success,
                    Message = new { User = user, Token = userToken.AccessToken, RefreshToken = userToken.RefreshToken }
                }); 

            }
            catch (Exception ex)
            {
                logger.LogError($"Error Occured while trying to get the User Details: {ex.Message}. Stack Trace: {ex.StackTrace}");
                return new BadRequestObjectResult(ex);
            }
        }

        [HttpPost]
        [Route("RefreshToken")]
        public async Task<IActionResult> RefreshToken(RefershTokenEntities tokenModel)
        {
            if (tokenModel is null)
            {
                return BadRequest("Invalid client request");
            }

            string? accessToken = tokenModel.AccessToken;
            string? refreshToken = tokenModel.RefreshToken;

            var userToken = await IdentityServer4Client.RunRefreshAsync(refreshToken!);
            var sessionExpireDate = (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) + userToken.ExpiresIn;
            return Ok(new GoveResponse
            {
                Status = Status.Success,
                Message = new { SessionExpireDate = sessionExpireDate, AccessToken = userToken.AccessToken, RefreshToken = userToken.RefreshToken }
            });
        }
        [HttpGet("GetUserMenus/{userId}", Name = "UserMenus")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(byte[]), StatusCodes.Status200OK, Web.ContentType.Json)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest, Web.ContentType.Json)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status500InternalServerError, Web.ContentType.Json)]
        public async Task<IActionResult> GetUserMenus(int? userId)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(userId, nameof(userId));
                var query = new GetUserMenusByUserId.Query(userId.Value);
                var userMenus = await GoveMediator.Send(query);
                return Ok(new GoveResponse
                {
                    Status = Status.Success,
                    Message = new { UserMenus = userMenus }
                });
            }
            catch (Exception ex)
            {
                return ErrorResponse(new Models.Responses.MessageResponse
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Error = new ErrorResponse { Exception = ex },
                    Request = userId
                });
            }
        }
    }
}
