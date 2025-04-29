using ABC.Management.Api.Commands;
using ABC.Management.Domain.Entities;
using ABC.PostGreSQL;
using Mediator;

namespace ABC.Management.Api.Handlers;

public class RemoveAntecedentHandler(
    IUnitOfWork _uow) : IRequestHandler<RemoveAntecedentCommand, BaseResponseCommand<Antecedent>>
{
    public async ValueTask<BaseResponseCommand<Antecedent>> Handle(
        RemoveAntecedentCommand request,
        CancellationToken cancellationToken)
    {
        var id = request.Entity.Id;
        BaseResponseCommand<Antecedent> response = new();        

        try
        {
            await _uow.Antecedents.RemoveAsync(id, cancellationToken);
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
                .SetCode(nameof(RemoveAntecedentHandler))
                .SetException(ex)
                .Build());
        }

        return response;
    }
}
