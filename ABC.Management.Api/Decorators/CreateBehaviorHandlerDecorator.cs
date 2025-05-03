using ABC.Management.Api.Commands;
using ABC.Management.Domain.Entities;
using FluentValidation;
using Mediator;

namespace ABC.Management.Api.Decorators;

public class CreateBehaviorHandlerDecorator(
    IValidator<Behavior> _validator,
    ILogger<ErrorValidationDecorator> _logger)
    : IPipelineBehavior<CreateBehaviorResponseCommand, BaseResponseCommand<Behavior>>
{
    public async ValueTask<BaseResponseCommand<Behavior>> Handle(
        CreateBehaviorResponseCommand message,
        MessageHandlerDelegate<CreateBehaviorResponseCommand, BaseResponseCommand<Behavior>> next,
        CancellationToken cancellationToken) =>
    
        await ErrorValidationDecorator.Handle(
            _validator,
            _logger,
            message,
            next,
            cancellationToken);
    
}