using AutoMapper;
using GOVE.Infrastructure.Queries;
using GOVE.Models.Requests;
using GOVE.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static GOVE.Infrastructure.Queries.GetNpaReportdetailsQuery;
using static GOVE.Models.Constants.Constants;
using static GOVE.Models.Responses.Response;

namespace GOVE.Admin.Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : BaseController
    {
        private readonly IConfiguration _configuration;
        public ReportsController(IConfiguration configuration, IMediator mediator, IMapper mapper, ILogger<UserController> logger) : base(mediator, logger, mapper)
        {
            _configuration = configuration;
        }
        [HttpPost]
        [Route("GetBrachLookup")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(byte[]), StatusCodes.Status200OK, Web.ContentType.Json)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest, Web.ContentType.Json)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status500InternalServerError, Web.ContentType.Json)]
        public async Task<IActionResult> GetBranchLookup(BranchLookupRequest BranchLookup)
        {
            try
            {
                var query = new GetBranchLookupQuery.Query(BranchLookup.UserId, BranchLookup.CompanyId, BranchLookup.Options);
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
        [Route("GetNpareportdetails")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(byte[]), StatusCodes.Status200OK, Web.ContentType.Json)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest, Web.ContentType.Json)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status500InternalServerError, Web.ContentType.Json)]
        public async Task<IActionResult> GetNpareportdetails(NpareportdetailsRequest nparequest)
        {
            try
            {
                var query = new GetNpaReportdetailsQuery.Query(nparequest.UserId, nparequest.CompanyId,nparequest.CutoffDate);
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
        [Route("GetNpaSummarydetails")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(byte[]), StatusCodes.Status200OK, Web.ContentType.Json)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest, Web.ContentType.Json)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status500InternalServerError, Web.ContentType.Json)]
        public async Task<IActionResult> GetNpaSummarydetails(NpasummaryreportdetailsRequest npasummaryrequest)
        {
            try
            {
                var query = new GetNpasummaryReportdetailsQuery.Query(npasummaryrequest.UserId, npasummaryrequest.CompanyId, npasummaryrequest.Branch, npasummaryrequest.Segment, npasummaryrequest.Subsegment);
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
    }
}
