using ABC.Management.Api.Commands;
using ABC.Management.Domain.Entities;
using ABC.PostGreSQL;
using FluentValidation;
using Mediator;

namespace ABC.Management.Api.Handlers;

public class CreateBehaviorHandler(
    UnitOfWork _uow,
    IValidator<Behavior> _behaviorValidator)
    : IRequestHandler<CreateBehaviorCommand, BaseResponseCommand<Behavior>>
{
    public async ValueTask<BaseResponseCommand<Behavior>> Handle(
        CreateBehaviorCommand request,
        CancellationToken cancellationToken)
    {
        BaseResponseCommand<Behavior> response = new();

        var validationResult = await _behaviorValidator
            .ValidateAsync(request.Value, cancellationToken);

        var errors = validationResult
            .Errors
            .Select(e =>
            ErrorBuilder
            .New()
            .SetMessage(e.ErrorMessage)
            .SetCode(e.ErrorCode)
            .Build());

        response.Errors = [.. errors];
        if (!validationResult.IsValid)
        {
            return response;
        }

        try
        {
            response.Entity = await _uow.Behaviors.AddAsync(request.Value, cancellationToken);
            var count = await _uow.SaveChangesAsync();
            if (count == 0)
            {
                throw new InvalidOperationException("Nothing saved to database");
            }
        }
        catch (Exception ex)
        {
            response.Errors.Add(
                ErrorBuilder.New()
                .SetException(ex)
                .Build());
        }

        return response;
    }
}
