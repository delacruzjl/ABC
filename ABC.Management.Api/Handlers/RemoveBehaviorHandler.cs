using ABC.Management.Api.Commands;
using ABC.Management.Domain.Entities;
using ABC.PostGreSQL;
using Mediator;

namespace ABC.Management.Api.Handlers;

public class RemoveBehaviorHandler(
    IUnitOfWork _uow) : IRequestHandler<RemoveBehaviorCommand, BaseResponseCommand<Behavior>>
{
    public async ValueTask<BaseResponseCommand<Behavior>> Handle(
        RemoveBehaviorCommand request,
        CancellationToken cancellationToken)
    {
        var id = request.Entity.Id;
        BaseResponseCommand<Behavior> response = new();

        try
        {
            await _uow.Behaviors.RemoveAsync(id, cancellationToken);
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
                .SetMessage("Error removing antecedent")
                .SetCode(nameof(RemoveBehaviorHandler))
                .SetException(ex)
                .Build());
        }

        return response;
    }
}
