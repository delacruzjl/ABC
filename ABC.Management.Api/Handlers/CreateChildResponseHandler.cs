using ABC.Management.Api.Commands;
using ABC.Management.Domain.Entities;
using ABC.PostGreSQL;
using FluentValidation;
using Mediator;

namespace ABC.Management.Api.Handlers;

public class CreateChildResponseHandler(
    IUnitOfWork _uow,
    IValidator<Child> _childValidator,
    ILogger<CreateChildResponseHandler> _logger)
    : IRequestHandler<CreateChildResponseCommand, BaseResponseCommand<Child>>
{
    public async ValueTask<BaseResponseCommand<Child>> Handle(
        CreateChildResponseCommand request,
        CancellationToken cancellationToken)
    {
        BaseResponseCommand<Child> response = new();

        try
        {
            await _childValidator
                .ValidateAndThrowAsync(request.Value, cancellationToken);

            response.Entity = await _uow.Children.AddAsync(request.Value, cancellationToken);
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
            _logger.LogError(ex, "Error creating child: {Message}", ex.Message);
            response.Errors.Add(
                ErrorBuilder.New()
                .SetException(ex)
                .SetMessage("Error creating child")
                .SetCode("ChildCreateError")
                .Build());
        }

        return response;
    }
}
