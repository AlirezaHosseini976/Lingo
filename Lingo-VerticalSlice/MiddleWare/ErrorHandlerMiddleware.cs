using System.Net;
using System.Text;
using FluentValidation;
using Lingo_VerticalSlice.Exceptions;
using Lingo_VerticalSlice.Shared;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Lingo_VerticalSlice.MiddleWare;

public class ErrorHandler
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandler> _logger;
    private ProblemDetails _problemDetails;

    public ErrorHandler(RequestDelegate next, ILogger<ErrorHandler> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            
            await _next(httpContext);
            
            if (httpContext.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
            {
                _problemDetails = new ProblemDetails()
                {
                    Type = "https://datatracker.ietf.org/doc/html/rfc7235#section-3.1",
                    Instance = httpContext.Request.Path,
                    Status = (int)HttpStatusCode.Unauthorized,
                    Title = Error.UnAuthorized.Code,
                    Detail = Error.UnAuthorized.Message
                };

                await WriteToResponseAsync(_problemDetails, HttpStatusCode.Unauthorized);
            }
        }
        catch (ValidationException e)
        {
            _logger.LogError(e, e.Message);
            _problemDetails = new ValidationProblemDetails(e.Errors
                .Where(x => !string.IsNullOrWhiteSpace(x.PropertyName))
                .GroupBy(x => x.PropertyName)
                .Select(x => { return new { key = x.Key, errors = x.Select(y => y.ErrorMessage) }; })
                .ToDictionary(d => d.key.ToString(), f => f.errors.ToArray()))
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
                Instance = httpContext.Request.Path,
                Status = (int)HttpStatusCode.BadRequest,
                Title = "Validation error occur!",
                Detail = "Please refer to the error property for additional details."
            };
            await WriteToResponseAsync(_problemDetails, HttpStatusCode.BadRequest);
        }

        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            _problemDetails = new ProblemDetails()
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
                Instance = httpContext.Request.Path,
                Status = (int)HttpStatusCode.InternalServerError,
                Title = Error.InternalException.Code.ToString(),
                Detail = Error.InternalException.Message
            };

            await WriteToResponseAsync(_problemDetails, HttpStatusCode.InternalServerError);
        }

        async Task WriteToResponseAsync(object result, HttpStatusCode httpStatusCode)
        {
            if (httpContext.Response.HasStarted)
                throw new InvalidOperationException(
                    "The response has already started, the http status code middleware will not be executed.");
            var json = JsonConvert.SerializeObject(result, new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            httpContext.Response.StatusCode = (int)httpStatusCode;
            httpContext.Response.ContentType = "application/json";
            await httpContext.Response.WriteAsync(json);
        }
    }
}