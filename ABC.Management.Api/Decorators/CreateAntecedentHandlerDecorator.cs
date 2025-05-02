using ABC.Management.Api.Commands;
using ABC.Management.Api.Handlers;
using ABC.Management.Domain.Entities;
using FluentValidation;
using Mediator;

namespace ABC.Management.Api.Decorators;

public class CreateAntecedentHandlerDecorator(
    IValidator<Antecedent> _antecedentValidator,
    ILogger<CreateAntecedentResponseHandler> _logger)
    : IPipelineBehavior<CreateAntecedentResponseCommand, BaseResponseCommand<Antecedent>> 
{
    public async ValueTask<BaseResponseCommand<Antecedent>> Handle(
        CreateAntecedentResponseCommand message,
        MessageHandlerDelegate<CreateAntecedentResponseCommand, BaseResponseCommand<Antecedent>> next,
        CancellationToken cancellationToken)
    {
        BaseResponseCommand<Antecedent> response = new();
        try
        {
            await _antecedentValidator
                .ValidateAndThrowAsync(message.Value, cancellationToken: cancellationToken);

            response = await next.Invoke(message, cancellationToken);
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning(ex, "Validation error creating antecedent: {Message}", ex.Message);

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
            _logger.LogError(ex, "Error creating antecedent: {Message}", ex.Message);
            response.Errors.Add(
                ErrorBuilder.New()
                .SetMessage("Error creating antecedent")
                .SetCode("AntecedentCreateError")
                .SetException(ex)
                .Build());
        }

        return response;
    }
}
