using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.AuthorizeAndAuthentication.JwtTokenHandlerInterface.AuthenticationsInterfaces;
using Microsoft.AspNetCore.Http;

namespace Persistence.Authorize
{
    internal class UserIdService : IUserIdService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserIdService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> GetUserIdAsync(CancellationToken cancellationToken = default)
        {
            return await Task.Run(() =>
            {
                var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                    throw new UnauthorizedAccessException("User ID not found in the token.");
                return userId;
            }, cancellationToken);
        }

    }
}
