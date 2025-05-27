using ABC.Management.Api.Commands;
using ABC.Management.Domain.Entities;
using Mediator;

namespace ABC.Management.Api.Decorators;

public class StartObservationHandlerDecorator(
    ILogger<ErrorValidationDecorator> _logger)
    : IPipelineBehavior<StartObservationCommand, BaseResponseCommand<Observation>>
{   

    public async ValueTask<BaseResponseCommand<Observation>> Handle(
        StartObservationCommand message,
        MessageHandlerDelegate<StartObservationCommand, BaseResponseCommand<Observation>> next,
        CancellationToken cancellationToken) =>
        await ErrorValidationDecorator.Handle(
            _logger,
            message,
            next,
            cancellationToken);
}