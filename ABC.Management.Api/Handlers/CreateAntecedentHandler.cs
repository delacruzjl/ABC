using ABC.Management.Api.Commands;
using ABC.Management.Domain.Entities;
using ABC.PostGreSQL;
using FluentValidation;
using Mediator;

namespace ABC.Management.Api.Handlers;

public class CreateAntecedentHandler(
    UnitOfWork _uow,
    IValidator<Antecedent> _antecedentValidator)
    : IRequestHandler<CreateAntecedentCommand, BaseResponseCommand<Antecedent>>
{

    public async ValueTask<BaseResponseCommand<Antecedent>> Handle(
        CreateAntecedentCommand request,
        CancellationToken cancellationToken)
    {
        BaseResponseCommand<Antecedent> response = new();

        var validationResult = await _antecedentValidator
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
            response.Entity = await _uow.Antecedents.AddAsync(request.Value, cancellationToken);
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
                .SetMessage("Error creating antecedent")
                .SetCode("AntecedentCreateError")
                .SetException(ex)
                .Build());
        }

        return response;
    }
}
