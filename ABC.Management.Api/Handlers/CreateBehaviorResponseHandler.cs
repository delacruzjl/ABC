using ABC.Management.Api.Commands;
using ABC.Management.Domain.Entities;
using ABC.PostGreSQL;
using FluentValidation;
using Mediator;

namespace ABC.Management.Api.Handlers;

public class CreateBehaviorResponseHandler(
    IUnitOfWork _uow,
    IValidator<Behavior> _behaviorValidator,
    ILogger<CreateBehaviorResponseHandler> _logger)
    : IRequestHandler<CreateBehaviorResponseCommand, BaseResponseCommand<Behavior>>
{
    public async ValueTask<BaseResponseCommand<Behavior>> Handle(
        CreateBehaviorResponseCommand request,
        CancellationToken cancellationToken)
    {
        BaseResponseCommand<Behavior> response = new();
        try
        {
            await _behaviorValidator
                .ValidateAndThrowAsync(request.Value, cancellationToken);

            response.Entity = await _uow.Behaviors.AddAsync(request.Value, cancellationToken);
            var count = await _uow.SaveChangesAsync();
            if (count == 0)
            {
                throw new InvalidOperationException("Nothing saved to database");
            }
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
            _logger.LogError(ex, "Error creating behavior: {Message}", ex.Message);
            response.Errors.Add(
                ErrorBuilder.New()
                .SetMessage("Error creating behavior")
                .SetCode("BehaviorCreateError")
                .SetException(ex)
                .Build());
        }

        return response;
    }
}
