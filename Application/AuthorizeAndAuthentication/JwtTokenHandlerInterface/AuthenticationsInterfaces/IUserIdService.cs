using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.AuthorizeAndAuthentication.JwtTokenHandlerInterface.AuthenticationsInterfaces
{
    public interface IUserIdService
    {
        Task<string> GetUserIdAsync(CancellationToken cancellationToken = default);
        // string GetTenantId();
    }

}
