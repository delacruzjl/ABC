using ABC.Management.Api.Commands;
using ABC.Management.Domain.Entities;
using FluentValidation;
using Mediator;

namespace ABC.Management.Api.Decorators;

public class CreateChildConditionHandlerDecorator(
    IValidator<ChildCondition> _validator,
    ILogger<ErrorValidationDecorator> _logger)
    : IPipelineBehavior<CreateChildConditionResponseCommand, BaseResponseCommand<ChildCondition>>
{
    public async ValueTask<BaseResponseCommand<ChildCondition>> Handle(
        CreateChildConditionResponseCommand message,
        MessageHandlerDelegate<CreateChildConditionResponseCommand, BaseResponseCommand<ChildCondition>> next,
        CancellationToken cancellationToken) =>

         await ErrorValidationDecorator.Handle(
            _validator,
            _logger,
            message,
            next,
            cancellationToken);

}

