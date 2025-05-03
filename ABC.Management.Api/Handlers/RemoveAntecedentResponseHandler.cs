using ABC.Management.Api.Commands;
using ABC.Management.Domain.Entities;
using ABC.PostGreSQL;
using Mediator;

namespace ABC.Management.Api.Handlers;

public class RemoveAntecedentHandler(
    IUnitOfWork _uow) : IRequestHandler<RemoveAntecedentResponseCommand, BaseResponseCommand<Antecedent>>
{
    public async ValueTask<BaseResponseCommand<Antecedent>> Handle(
        RemoveAntecedentResponseCommand request,
        CancellationToken cancellationToken)
    {
        var id = request.Entity.Id;
        BaseResponseCommand<Antecedent> response = new();
        
        await _uow.Antecedents.RemoveAsync(id, cancellationToken);        
        var count = await _uow.SaveChangesAsync();

        if (count == 0)
        {
            throw new InvalidOperationException("Nothing saved to database");
        }

        return response;
    }
}
