using Application.ExceptionTypes;
using Infrastructure.Exceptions.Types;

namespace Infrastructure.Exceptions.Handlers;

public abstract class ExceptionHandler
{
    public Task HandleExceptionAsync(Exception exception) => exception switch

    {
        BusinessException businessException => HandleException(businessException),
        ValidationException validationException => HandleException(validationException),
        AuthorizationException authorizationException => HandleException(authorizationException),
        AuthenticationException authenticationException => HandleException(authenticationException),
        NotFoundException notFoundException => HandleException(notFoundException),
        OperationException operationException => HandleException(operationException),

        _ => HandleException(exception)
    };

    protected abstract Task HandleException(BusinessException businessException);
    protected abstract Task HandleException(ValidationException validationException);
    protected abstract Task HandleException(AuthorizationException authorizationException);
    protected abstract Task HandleException(AuthenticationException authenticationException);
    protected abstract Task HandleException(NotFoundException notFoundException);
    protected abstract Task HandleException(OperationException operationException);
    protected abstract Task HandleException(Exception exception);

}
