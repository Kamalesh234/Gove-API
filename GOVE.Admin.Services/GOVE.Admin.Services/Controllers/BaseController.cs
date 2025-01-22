using AutoMapper;
using GOVE.Models.Exceptions;
using GOVE.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


namespace GOVE.Admin.Services.Controllers;

public class BaseController(IMediator mediator, ILogger<BaseController> logger) : ControllerBase
{
    public BaseController(IMediator mediator, ILogger<BaseController> logger, IMapper mapper) : this(mediator, logger) => HLObjectMapper = mapper;
    protected IMediator GoveMediator => mediator;
    private IMapper HLObjectMapper { get; } = default!;
    protected IActionResult ErrorResponse(MessageResponse response)
    {
        var methodName = $"{GetType().Name}.{nameof(ErrorResponse)}";

        logger.LogInformation("{MethodName}: Message handler response status is not OK", methodName);
        logger.LogDebug("{MethodName}: Error in message handler: {Error}", methodName, JsonConvert.SerializeObject(response.Error));

        if (response.IsRequestValid) return BuildErrorResponse(new MessageHandlerException(response));

        logger.LogInformation("{MethodName}: Request validation failed.", methodName);
        logger.LogDebug("{MethodName}: Validation errors: {ValidationErrors}", methodName, response.Error.ValidationErrors);

        return BuildErrorResponse(new ValidationException(response.Error.ValidationErrors));
    }
    private ObjectResult BuildErrorResponse(GoveException exception)
    {
        // Get the current method name for logging purposes
        var methodName = $"{GetType().Name}.{nameof(BuildErrorResponse)}";

        // Map the exception to a base response object
        var response = HLObjectMapper.Map<BaseResponse>(exception);

        // Handle different types of exceptions
        switch (exception)
        {
            case ValidationException validationEx:
                // Create a response for validation exceptions
                response = new BaseResponse
                {
                    StatusCode = validationEx.StatusCode,
                    Error = new ErrorResponse { Message = validationEx.Message, ValidationErrors = validationEx.ValidationErrors }
                };
                break;
            case MessageHandlerException messageHandlerEx:
                // Log the error for message handler exceptions
                logger.LogDebug("{MethodName}: Error in message handler: {Error}", methodName, JsonConvert.SerializeObject(messageHandlerEx));

                // Map the message handler exception to a response
                response = HLObjectMapper.Map<BaseResponse>(messageHandlerEx);
                response.Error.AdditionalData = new Dictionary<string, object>
                {
                    { "requestType", messageHandlerEx.Response.RequestType?.Name ?? String.Empty },
                    { "request", JsonConvert.SerializeObject(messageHandlerEx.Response.Request)}
                };
                break;
            default:
                // Map the exception to an inner error response for other exception types
                response.Error.InnerError = HLObjectMapper.Map<ErrorResponse>(exception);
                break;
        }

        // Return the error response with the appropriate status code
        return StatusCode((int)response.StatusCode, response);
    }

}
