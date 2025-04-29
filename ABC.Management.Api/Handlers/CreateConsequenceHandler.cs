using ABC.Management.Api.Commands;
using ABC.Management.Domain.Entities;
using ABC.Management.Domain.Validators;
using ABC.PostGreSQL;
using FluentValidation;
using Mediator;

namespace ABC.Management.Api.Handlers;

public class CreateConsequenceHandler(
    IUnitOfWork _uow,
    IValidator<Consequence> _consequenceValidator)
    : IRequestHandler<CreateConsequenceCommand, BaseResponseCommand<Consequence>>
{
    public async ValueTask<BaseResponseCommand<Consequence>> Handle(
        CreateConsequenceCommand request,
        CancellationToken cancellationToken)
    {
        BaseResponseCommand<Consequence> response = new();

        var validationResult = await _consequenceValidator
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
            response.Entity = await _uow.Consequences.AddAsync(request.Value, cancellationToken);
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
                .SetMessage("Error creating consequence")
                .SetCode("ConsequenceCreateError")
                .Build());
        }

        return response;
    }
}
