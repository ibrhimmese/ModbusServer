using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Exceptions.HttpProblemDetails;

public class OperationExceptionProblemDetails:ProblemDetails
{
    public OperationExceptionProblemDetails(string detail)
    {
        Title = "Operation Exception";
        Detail = detail;
        Status = StatusCodes.Status500InternalServerError;
        Type = "http://example.com/probs/operation";
    }
}
