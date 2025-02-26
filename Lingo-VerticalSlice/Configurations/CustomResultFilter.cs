using Lingo_VerticalSlice.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Lingo_VerticalSlice.Configurations;

public class CustomResultFilter : ActionFilterAttribute
{
    public override void OnResultExecuting(ResultExecutingContext context)
    { 
        if (context.Result is ObjectResult objectResult)
        {
            if (objectResult.Value is Result result)
            {
                if (result is { IsSuccess: false })
                {
                    var error = result.Error;
                    var problemDetails = new ProblemDetails
                    {
                        Title = error.Code,
                        Detail = error.Message,
                        Instance = context.HttpContext.Request.Path
                    };
                    context.Result = new ObjectResult(problemDetails)
                    {
                        StatusCode = objectResult.StatusCode
                    };
                }
            }
        }
        base.OnResultExecuting(context);
    }
}