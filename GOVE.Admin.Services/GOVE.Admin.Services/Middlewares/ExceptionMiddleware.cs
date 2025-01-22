using AutoMapper;
using Gove.Infrastructure.Utils;
using GOVE.Infrastructure.Utils;
using GOVE.Models.Exceptions;
using GOVE.Models.Responses;
using static GOVE.Models.Constants.Constants;

namespace GOVE.Admin.Services.Middlewares;

public class ExceptionMiddleware
{
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IMapper _mapper;
    private readonly RequestDelegate _next;
    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IMapper mapper)
    {
        _next = next;
        _logger = logger;
        _mapper = mapper;
    }
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }
    private async Task HandleExceptionAsync(HttpContext httpContext, Exception sourceEx)
    {
        var methodName = $"{GetType().Name}.{AppUtil.GetCurrentMethodName()}";

        _logger.LogInformation("{MethodName}: Exception thrown, handling and returning error response", methodName);

        // #Localize
        var response = _mapper.Map<BaseResponse>(sourceEx);

        switch (sourceEx)
        {
            case ValidationException validationEx:
                _logger.LogDebug("{MethodName}: Validation error: {Error}", methodName, validationEx.ToJson());

                response.Error.ValidationErrors = validationEx.ValidationErrors;
                break;

            case ConfigurationException configurationEx:
                _logger.LogDebug("{MethodName}: Error in configuration: {Error}", methodName, configurationEx.ToJson());
                break;

            case MessageHandlerException messageHandlerEx:
                _logger.LogDebug("{MethodName}: Error in message handler: {Error}", methodName, messageHandlerEx.ToJson());

                response = _mapper.Map<BaseResponse>(messageHandlerEx);
                response.Error.AdditionalData = new Dictionary<string, object>
                {
                    {
                        "requestType", messageHandlerEx.Response.RequestType?.Name ?? String.Empty
                    },
                    {
                        "request", messageHandlerEx.Response.Request.ToJson()
                    }
                };
                break;

            case RestException hlRestEx:
                _logger.LogDebug("{MethodName}: Error in REST service: {Error}", methodName, hlRestEx.ToJson());

                response = _mapper.Map<BaseResponse>(hlRestEx);
                response.Error.InnerError.AdditionalData = new Dictionary<string, object>
                {
                    {
                        "responseData", hlRestEx.ResponseData!
                    }
                };

                break;

            case { }:
                response.Error.InnerError = _mapper.Map<ErrorResponse>(sourceEx);
                break;
        }

        httpContext.Response.ContentType = Web.ContentType.Json;
        httpContext.Response.StatusCode = (int)response.StatusCode;

        var contextResponse = response.ToJson();

        _logger.LogDebug("{MethodName}: Error response: {ErrorResponse}", methodName, contextResponse);
        _logger.LogDebug("{MethodName}: Writing error response", methodName);

        await httpContext.Response.WriteAsync(contextResponse);
    }
}
