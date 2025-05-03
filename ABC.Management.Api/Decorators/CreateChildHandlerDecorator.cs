using ABC.Management.Api.Commands;
using ABC.Management.Domain.Entities;
using FluentValidation;
using Mediator;

namespace ABC.Management.Api.Decorators;

public class CreateChildHandlerDecorator(
    IValidator<Child> _validator,
    ILogger<ErrorValidationDecorator> _logger)
    : IPipelineBehavior<CreateChildResponseCommand, BaseResponseCommand<Child>>
{
    public async ValueTask<BaseResponseCommand<Child>> Handle(
        CreateChildResponseCommand message,
        MessageHandlerDelegate<CreateChildResponseCommand, BaseResponseCommand<Child>> next,
        CancellationToken cancellationToken) =>
    
        await ErrorValidationDecorator.Handle(
            _validator,
            _logger,
            message,
            next,
            cancellationToken);
    
}

