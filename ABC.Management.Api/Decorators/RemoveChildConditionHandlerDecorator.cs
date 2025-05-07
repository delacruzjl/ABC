using ABC.Management.Api.Commands;
using ABC.Management.Domain.Entities;
using Mediator;

namespace ABC.Management.Api.Decorators;

public class RemoveChildConditionHandlerDecorator(
    ILogger<ErrorValidationDecorator> _logger)
    : IPipelineBehavior<RemoveChildConditionResponseCommand, BaseResponseCommand<ChildCondition>>
{
    public async ValueTask<BaseResponseCommand<ChildCondition>> Handle(
        RemoveChildConditionResponseCommand message,
        MessageHandlerDelegate<RemoveChildConditionResponseCommand, BaseResponseCommand<ChildCondition>> next,
        CancellationToken cancellationToken) =>
     await ErrorValidationDecorator.Handle(
            _logger,
            message,
            next,
            cancellationToken);

}

