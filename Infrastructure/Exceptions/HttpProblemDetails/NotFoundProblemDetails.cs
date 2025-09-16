using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Exceptions.HttpProblemDetails
{
    public class NotFoundProblemDetails:ProblemDetails
    {
        public NotFoundProblemDetails(string detail)
        {
            Title = "404 Not Found";
            Detail = detail;
            Status = StatusCodes.Status404NotFound;
            Type = "http://example.com/probs/404-not-found";
        }
    }
}
