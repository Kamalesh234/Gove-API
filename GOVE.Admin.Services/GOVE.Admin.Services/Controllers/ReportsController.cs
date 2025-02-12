using AutoMapper;
using AutoMapper.Internal;
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
                var query = new GetNpaReportdetailsQuery.Query(nparequest.UserId, nparequest.CompanyId,nparequest.CutoffDate,nparequest.Accountnumber);
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
                var query = new GetNpasummaryReportdetailsQuery.Query(npasummaryrequest.UserId, npasummaryrequest.CompanyId, npasummaryrequest.Branch, npasummaryrequest.Segment, npasummaryrequest.Subsegment, npasummaryrequest.CutoffDate, npasummaryrequest.Accountnumber,npasummaryrequest.Options);
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
        [Route("GetNpaHistorydetails")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(byte[]), StatusCodes.Status200OK, Web.ContentType.Json)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest, Web.ContentType.Json)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status500InternalServerError, Web.ContentType.Json)]
        public async Task<IActionResult> GetNpaHistorydetails(NpahistoryreportdetailsRequest npahistoryrequest)
        {
            try
            {
                var query = new GetNpahistoryReportdetailsQuery.Query(npahistoryrequest.UserId, npahistoryrequest.CompanyId, npahistoryrequest.FromDate, npahistoryrequest.ToDate,npahistoryrequest.Accountnumber);
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
        [Route("GetEmployeeCodeLookup")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(byte[]), StatusCodes.Status200OK, Web.ContentType.Json)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest, Web.ContentType.Json)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status500InternalServerError, Web.ContentType.Json)]

        public async Task<IActionResult> GetEmployeeCodeLookup()
        {
            try
            {
                var query = new GetEmployeeCodeLookups.Query();
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
        [Route("GetSMAReportdetails")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(byte[]), StatusCodes.Status200OK, Web.ContentType.Json)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest, Web.ContentType.Json)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status500InternalServerError, Web.ContentType.Json)]
        public async Task<IActionResult> GetSMAReportdetails(NpasmareportdetailsRequest npasmarequest)
        {
            try
            {
                var query = new GetSMAReportdetailsQuery.Query(npasmarequest.UserId, npasmarequest.CompanyId, npasmarequest.Branch, npasmarequest.SMAType, npasmarequest.EmployeeCode);
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
