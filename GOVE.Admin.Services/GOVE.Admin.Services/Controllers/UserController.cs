using Microsoft.AspNetCore.Mvc;
using MediatR;
using AutoMapper;
using GOVE.Models.Responses;
using static GOVE.Models.Constants.Constants;
using GOVE.Models.Requests;
using static GOVE.Models.Responses.Response;
using GOVE.Infrastructure.Queries;

namespace GOVE.Admin.Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IConfiguration _configuration;
        public UserController(IConfiguration configuration, IMediator mediator, IMapper mapper, ILogger<UserController> logger) : base(mediator, logger, mapper)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [Route("UserTranslander")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(byte[]), StatusCodes.Status200OK, Web.ContentType.Json)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest, Web.ContentType.Json)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status500InternalServerError, Web.ContentType.Json)]
        public async Task<IActionResult> UserTranslander(UserTranslanderRequest UserTranslanderRequest)
        {
            try
            {
                var query = new UsertranslanderQuery.Query(UserTranslanderRequest.UserId, UserTranslanderRequest.CompanyId);
                var existingUserDetails = await GoveMediator.Send(query);
                return Ok(new GoveResponse
                {
                    Status = Status.Success,
                    Message = existingUserDetails
                });
            }
            catch (Exception ex)
            {
                return ErrorResponse(new Models.Responses.MessageResponse
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Error = new ErrorResponse { Exception = ex }

                });
            }
        }

        [HttpPost]
        [Route("GetUserDetails")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(byte[]), StatusCodes.Status200OK, Web.ContentType.Json)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest, Web.ContentType.Json)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status500InternalServerError, Web.ContentType.Json)]
        public async Task<IActionResult> GetUserDetails(GetUserDetailsRequest getUserrequest)
        {
            try
            {
                //var query = new ViewUserDetails.Query(getUserrequest.UserId, getUserrequest.CompanyId);
                //var existingUserDetails = await GoveMediator.Send(query);

                return Ok(new GoveResponse
                {
                    Status = Status.Success
                    //Message = existingUserDetails
                });
            }
            catch (Exception ex)
            {
                return ErrorResponse(new Models.Responses.MessageResponse
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Error = new ErrorResponse { Exception = ex }

                });
            }

        }
    }
}
