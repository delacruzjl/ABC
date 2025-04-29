using ABC.Management.Api.Commands;
using ABC.Management.Domain.Entities;
using ABC.PostGreSQL;
using Mediator;

namespace ABC.Management.Api.Handlers;

public class RemoveChildHandler(
    UnitOfWork _uow) : IRequestHandler<RemoveChildCommand, BaseResponseCommand<Child>>
{
    public async ValueTask<BaseResponseCommand<Child>> Handle(
        RemoveChildCommand request,
        CancellationToken cancellationToken)
    {
        var id = request.Entity.Id;
        BaseResponseCommand<Child> response = new();

        try
        {
            await _uow.Children.RemoveAsync(id, cancellationToken);
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
                .SetMessage("Error removing Child")
                .SetCode(nameof(RemoveChildHandler))
                .SetException(ex)
                .Build());
        }

        return response;
    }
}
