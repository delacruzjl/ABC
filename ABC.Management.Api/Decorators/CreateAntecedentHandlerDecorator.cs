using ABC.Management.Api.Commands;
using ABC.Management.Domain.Entities;
using FluentValidation;
using Mediator;

namespace ABC.Management.Api.Decorators;

public class CreateAntecedentHandlerDecorator(
    IValidator<Antecedent> _antecedentValidator,
    ILogger<ErrorValidationDecorator> _logger)
    : IPipelineBehavior<CreateAntecedentResponseCommand, BaseResponseCommand<Antecedent>> 
{
    public async ValueTask<BaseResponseCommand<Antecedent>> Handle(
        CreateAntecedentResponseCommand message,
        MessageHandlerDelegate<CreateAntecedentResponseCommand, BaseResponseCommand<Antecedent>> next,
        CancellationToken cancellationToken) =>
    
         await ErrorValidationDecorator.Handle(
            _antecedentValidator,
            _logger,
            message,
            next,
            cancellationToken);
    
}

