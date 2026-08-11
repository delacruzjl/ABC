using System.Diagnostics;
using HotChocolate;
using HotChocolate.Execution;

namespace ABC.Management.Api.Extensions;

public class CorrelatedErrorFilter(ILogger<CorrelatedErrorFilter> logger) : IErrorFilter
{
    public IError OnError(IError error)
    {
        // Preserve errors that already have a user-facing code (e.g. AUTH_NOT_OWNER, validation)
        if (error.Code is not null && error.Exception is null)
            return error;

        var correlationId = Activity.Current?.Id ?? Guid.CreateVersion7().ToString();

        // Log the full exception for diagnostics
        if (error.Exception is not null)
        {
            logger.LogError(
                error.Exception,
                "Unhandled GraphQL error [CorrelationId={CorrelationId}]",
                correlationId);
        }
        else
        {
            logger.LogError(
                "GraphQL error without exception [CorrelationId={CorrelationId}]: {Message}",
                correlationId,
                error.Message);
        }

        return ErrorBuilder.FromError(error)
            .SetMessage($"Something went wrong. Please try again or contact support. (Reference: {correlationId})")
            .SetCode("INTERNAL_ERROR")
            .SetExtension("correlationId", correlationId)
            .Build();
    }
}
