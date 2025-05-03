using ABC.Management.Api.Commands;
using ABC.Management.Domain.Entities;
using ABC.PostGreSQL;
using FluentValidation;
using Mediator;

namespace ABC.Management.Api.Handlers;

public class CreateBehaviorResponseHandler(
    IUnitOfWork _uow)
    : IRequestHandler<CreateBehaviorResponseCommand, BaseResponseCommand<Behavior>>
{
    public async ValueTask<BaseResponseCommand<Behavior>> Handle(
        CreateBehaviorResponseCommand request,
        CancellationToken cancellationToken)
    {
        BaseResponseCommand<Behavior> response = new()
        {
            Entity = await _uow.Behaviors.AddAsync(request.Value, cancellationToken)
        };

        var count = await _uow.SaveChangesAsync();
        if (count == 0)
        {
            throw new InvalidOperationException("Nothing saved to database");
        }

        return response;
    }
}
