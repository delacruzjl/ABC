using ABC.Management.Api.Commands;
using ABC.Management.Domain.Entities;
using FluentValidation;
using Mediator;

namespace ABC.Management.Api.Decorators;

public class CreateConsequenceHandlerDecorator(
    IValidator<Consequence> _validator,
    ILogger<ErrorValidationDecorator> _logger)
    : IPipelineBehavior<CreateConsequenceResponseCommand, BaseResponseCommand<Consequence>>
{
    public async ValueTask<BaseResponseCommand<Consequence>> Handle(
        CreateConsequenceResponseCommand message,
        MessageHandlerDelegate<CreateConsequenceResponseCommand, BaseResponseCommand<Consequence>> next,
        CancellationToken cancellationToken) =>
    await ErrorValidationDecorator.Handle(
            _validator,
            _logger,
            message,
            next,
            cancellationToken);
    
}

