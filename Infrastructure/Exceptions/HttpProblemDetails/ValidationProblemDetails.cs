using Infrastructure.Exceptions.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Exceptions.HttpProblemDetails;

public class ValidationProblemDetails : ProblemDetails
{
    public IEnumerable<ValidationExceptionModel> Errors { get; init; }

    public ValidationProblemDetails(IEnumerable<ValidationExceptionModel> errors)
    {
        Title = "Validation Error(s)";
        Detail = "One or more validation errors occurred.";
        Errors = errors;
        Status = StatusCodes.Status400BadRequest;
        Type = "https://example.com/probs/validation";
    }
}
