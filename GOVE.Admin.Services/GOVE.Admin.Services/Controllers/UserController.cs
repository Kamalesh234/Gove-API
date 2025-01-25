using Microsoft.AspNetCore.Mvc;
using MediatR;
using AutoMapper;
using GOVE.Models.Responses;
using static GOVE.Models.Constants.Constants;
using GOVE.Models.Requests;
using static GOVE.Models.Responses.Response;
using GOVE.Infrastructure.Queries;
using GOVE.Models.Entities;
using GOVE.Infrastructure.Commands;

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
                var query = new UserDetailsQuery.Query(getUserrequest.UserId, getUserrequest.CompanyId);
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
        [Route("GetUserlevelLookup")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(byte[]), StatusCodes.Status200OK, Web.ContentType.Json)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest, Web.ContentType.Json)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status500InternalServerError, Web.ContentType.Json)]

        public async Task<IActionResult> GetUserLevelLookup(UserManagementLookupRequest userManagementLookup)
        {
            try
            {
                var query = new UserLevelLookup.Query(userManagementLookup.UserId, userManagementLookup.CompanyId);
                var lookup = await GoveMediator.Send(query);

                return Ok(new GoveResponse
                {
                    Status = Status.Success,
                    Message = lookup
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
        [Route("GetUserdesignationlevel")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(byte[]), StatusCodes.Status200OK, Web.ContentType.Json)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest, Web.ContentType.Json)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status500InternalServerError, Web.ContentType.Json)]

        public async Task<IActionResult> GetUserdesignationlevelLookup(UserDesinationModelRequest UserDesination)
        {
            try
            {
                var query = new GetUserDesignationlevelLookups.Query(UserDesination.CompanyId);
                var lookup = await GoveMediator.Send(query);

                return Ok(new GoveResponse
                {
                    Status = Status.Success,
                    Message = lookup
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
        [Route("GetUserreportinglevel")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(byte[]), StatusCodes.Status200OK, Web.ContentType.Json)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest, Web.ContentType.Json)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status500InternalServerError, Web.ContentType.Json)]

        public async Task<IActionResult> GetUserreportingevelLookup(UserReportingLevelLookup UserReporting)
        {
            try
            {
                var query = new UserReportinglevel.Query(UserReporting.CompanyId, UserReporting.UserId, UserReporting.PrefixText);
                var lookup = await GoveMediator.Send(query);

                return Ok(new GoveResponse
                {
                    Status = Status.Success,
                    Message = lookup
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
        [HttpGet]
        [Route("GetProspectLookup")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(byte[]), StatusCodes.Status200OK, Web.ContentType.Json)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest, Web.ContentType.Json)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status500InternalServerError, Web.ContentType.Json)]

        public async Task<IActionResult> GetProspectLookup()
        {
            try
            {
                var query = new GetProspectLookups.Query();
                var lookup = await GoveMediator.Send(query);

                return Ok(new GoveResponse
                {
                    Status = Status.Success,
                    Message = lookup
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
        [Route("UserInsert")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(byte[]), StatusCodes.Status200OK, Web.ContentType.Json)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest, Web.ContentType.Json)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status500InternalServerError, Web.ContentType.Json)]

        public async Task<IActionResult> UserDetailsInsert(InsertUserDetailsModel newUserdetails)
        {
            try
            {
                var command = new InsertUserDetails.Command(newUserdetails);
                var response = await GoveMediator.Send(command);
                return GenerateResponse(response);
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
        private IActionResult GenerateResponse(int response)
        {
            if (response == (int)SaveStatus.OK)
            {
                return Ok(new GoveResponse
                {
                    Status = Status.Success,
                    Message = "User Created successfully"
                });
            }
            else
            {
                return BadRequest(new GoveResponse
                {
                    Status = Status.Error,
                    Message = "An error occurred while creating the user"
                });
            }

        }
    }
}
