using ABC.Management.Api.Commands;
using ABC.Management.Domain.Entities;
using ABC.PostGreSQL;
using FluentValidation;
using Mediator;

namespace ABC.Management.Api.Handlers;

public class CreateAntecedentResponseHandler(
    IUnitOfWork _uow,
    IValidator<Antecedent> _antecedentValidator,
    ILogger<CreateAntecedentResponseHandler> _logger)
    : IRequestHandler<CreateAntecedentResponseCommand, BaseResponseCommand<Antecedent>>
{

    public async ValueTask<BaseResponseCommand<Antecedent>> Handle(
        CreateAntecedentResponseCommand request,
        CancellationToken cancellationToken)
    {
        BaseResponseCommand<Antecedent> response = new();
        try
        {
            await _antecedentValidator
            .ValidateAndThrowAsync(request.Value, cancellationToken: cancellationToken);

            response.Entity = await _uow.Antecedents.AddAsync(request.Value, cancellationToken);
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
