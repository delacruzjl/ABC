using ABC.Management.Api.Commands;
using ABC.SharedKernel;
using FluentValidation;
using Mediator;

namespace ABC.Management.Api.Decorators;

public class ErrorValidationDecorator
{
    public static async ValueTask<BaseResponseCommand<TEntity>> Handle<TCommand, TEntity>(
        ILogger<ErrorValidationDecorator> _logger,
        TCommand message,
        MessageHandlerDelegate<TCommand, BaseResponseCommand<TEntity>> inner,
        CancellationToken cancellationToken = default)
        where TCommand : notnull, IMessage
        where TEntity : Entity
    {
        BaseResponseCommand<TEntity> response = new();

        try
        {
            response = await inner
                .Invoke(message, cancellationToken);
        }
        catch (Exception ex)
        {
            LogAndHandlePipelineError<TCommand, TEntity>(_logger, response, ex);
        }

        return response;
    }

    public static async ValueTask<BaseResponseCommand<TEntity>> Handle<TCommand, TEntity>(
        IValidator<TEntity> _validator,
        ILogger<ErrorValidationDecorator> _logger,
        TCommand message,
        MessageHandlerDelegate<TCommand, BaseResponseCommand<TEntity>> inner,
        CancellationToken cancellationToken = default)
        where TCommand : notnull, IMessage
        where  TEntity : Entity
    {
        BaseResponseCommand<TEntity> response = new();
        try
        {
            var entity = message.GetType()
                    .GetProperty("Value")?.GetValue(message) as TEntity;

            await _validator
                .ValidateAndThrowAsync(
                entity!,
                cancellationToken: cancellationToken);

            response = await inner
                .Invoke(message, cancellationToken);
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning(
                ex,
                "Validation error creating antecedent: {Message}",
                ex.Message);

            var errors = ex
                .Errors
                .Select(e =>
                ErrorBuilder
                .New()
                .SetMessage(e.ErrorMessage)
                .SetCode(e.ErrorCode)
                .Build());

            response.Errors = [.. errors];
        }
        catch (Exception ex)
        {
            LogAndHandlePipelineError<TCommand, TEntity>(_logger, response, ex);
        }

        return response;
    }

    private static void LogAndHandlePipelineError<TCommand, TEntity>(ILogger<ErrorValidationDecorator> _logger, BaseResponseCommand<TEntity> response, Exception ex)
        where TCommand : notnull, IMessage
        where TEntity : Entity
    {
        _logger.LogError(ex, "Error during pipeline: {Message}", ex.Message);
        response.Errors.Add(
            ErrorBuilder.New()
            .SetMessage($"Error creating entity of type {typeof(TEntity).Name}")
            .SetCode(typeof(TCommand).Name)
            .SetException(ex)
            .Build());
    }
}
