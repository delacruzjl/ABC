using ABC.Management.Api.Commands;
using ABC.Management.Domain.Entities;
using ABC.Management.Domain.Validators;
using ABC.PostGreSQL;
using FluentValidation;
using Mediator;

namespace ABC.Management.Api.Handlers;

public class CreateChildHandler(
    IUnitOfWork _uow,
    IValidator<Child> _childValidator)
    : IRequestHandler<CreateChildCommand, BaseResponseCommand<Child>>
{
    public async ValueTask<BaseResponseCommand<Child>> Handle(
        CreateChildCommand request,
        CancellationToken cancellationToken)
    {
        BaseResponseCommand<Child> response = new();

        var validationResult = await _childValidator
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
            response.Entity = await _uow.Children.AddAsync(request.Value, cancellationToken);
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
                .SetMessage("Error creating child")
                .SetCode("ChildCreateError")
                .Build());
        }

        return response;
    }
}
