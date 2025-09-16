using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Exceptions.HttpProblemDetails;

public class AuthenticationProblemDetails : ProblemDetails
{
    public AuthenticationProblemDetails(string details)
    {
        Title = "Authentication error";
        Detail = details;
        Status = StatusCodes.Status403Forbidden;
        Type = "https://example.com/probs/authentication";
    }
}