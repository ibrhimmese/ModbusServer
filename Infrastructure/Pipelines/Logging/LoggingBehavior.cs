using Infrastructure.Logging.Serilog;
using Infrastructure.Logging;
using MediatR;
using Microsoft.AspNetCore.Http;
using Application.Pipelines;
using System.Text.Json;

namespace ECommerce.Infrastructure.Pipelines.Logging;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
where TRequest : IRequest<TResponse>, ILoggableRequest
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly LoggerServiceBase _loggerServiceBase;

    public LoggingBehavior(IHttpContextAccessor httpContextAccessor, LoggerServiceBase loggerServiceBase)
    {
        _httpContextAccessor = httpContextAccessor;
        _loggerServiceBase = loggerServiceBase;
    }

    //public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    //{
    //    List<LogParameter> logParameters =
    //        new()
    //        {
    //    new LogParameter{Type= request.GetType().Name, Value= request },
    //        };

    //    LogDetail logDetail
    //        = new()
    //        {
    //            MethodName = next.Method.Name,
    //            Parameters = logParameters,
    //            User = _httpContextAccessor.HttpContext?.User.Identity?.Name ?? "?"
    //        };

    //    _loggerServiceBase.Info(JsonSerializer.Serialize(logDetail));
    //    return await next();
    //}


     public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var logParameters = new List<LogParameter>
        {
            new LogParameter { Type = request.GetType().Name, Value = request }
        };

        var user = _httpContextAccessor.HttpContext?.User.Identity?.Name ?? "?";
        var methodName = next.Method.Name;

        try
        {
            // Başarılı istek loglama
            var logDetail = new LogDetail
            {
                MethodName = methodName,
                Parameters = logParameters,
                User = user
            };

            _loggerServiceBase.Info(JsonSerializer.Serialize(logDetail));

            return await next();
        }
        catch (Exception ex)
        {
            // Hata loglama
            var logDetailWithException = new LogDetailWithException(
                fullName: typeof(TRequest).FullName ?? string.Empty,
                methodName: methodName,
                user: user,
                parameters: logParameters,
                exceptionMessage: ex.ToString()
            );

            _loggerServiceBase.Error(JsonSerializer.Serialize(logDetailWithException));

            throw; // Exception’ı tekrar fırlatıyoruz
        }
    }
}
