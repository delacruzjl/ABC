using ABC.Management.Api.Commands;
using ABC.Management.Domain.Entities;
using ABC.SharedEntityFramework;
using Mediator;

namespace ABC.Management.Api.Handlers;

public class CreateAntecedentResponseHandler(IUnitOfWork _uow)
    : IRequestHandler<CreateAntecedentResponseCommand, BaseResponseCommand<Antecedent>>
{

    public async ValueTask<BaseResponseCommand<Antecedent>> Handle(
        CreateAntecedentResponseCommand request,
        CancellationToken cancellationToken)
    {
        BaseResponseCommand<Antecedent> response = new()
        {
            Entity = await _uow.Antecedents.AddAsync(request.Value, cancellationToken)
        };

        var count = await _uow.SaveChangesAsync();
        if (count == 0)
        {
            throw new InvalidOperationException("Nothing saved to database");
        }

        return response;
    }
}
