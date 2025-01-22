using AutoMapper;
using GOVE.Infrastructure.Queries;
using GOVE.Models.Requests;
using GOVE.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static GOVE.Models.Constants.Constants;
using static GOVE.Models.Responses.Response;

namespace GOVE.Admin.Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : BaseController
    {
        public CompanyController(IMediator mediator, ILogger<CompanyController> logger, IMapper mapper) : base(mediator, logger, mapper)
        {
        }
        [HttpPost]
        [Route("GetCompanyMaster")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(byte[]), StatusCodes.Status200OK, Web.ContentType.Json)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest, Web.ContentType.Json)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status500InternalServerError, Web.ContentType.Json)]
        public async Task<IActionResult> GetCompanyMaster(GetCompanyRequestModel customerRequest)
        {
            try
            {
                var query = new GetCompanyMaster.Query(customerRequest.CompanyId);
                var existingProspectDetail = await GoveMediator.Send(query);
                return Ok(new GoveResponse
                {
                    Status = Status.Success,
                    Message = existingProspectDetail
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
