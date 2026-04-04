using ABC.Management.Api.Commands;
using ABC.Management.Domain.Entities;
using ABC.PostGreSQL.ValidationServices;
using ABC.SharedEntityFramework;
using Mediator;

namespace ABC.Management.Api.Handlers;

public class UpdateChildResponseHandler(
    IUnitOfWork _uow)
    : IRequestHandler<UpdateChildResponseCommand, BaseResponseCommand<Child>>
{
    public async ValueTask<BaseResponseCommand<Child>> Handle(
        UpdateChildResponseCommand request,
        CancellationToken cancellationToken)
    {
        var childQry = await _uow.Children
            .GetAsync(c => c.Id == request.ChildId, cancellationToken);

        var child = childQry.Single();

        Child updated = new(
            child.Id,
            request.LastName,
            request.FirstName,
            request.BirthYear,
            [])
        {
            UserId = request.UserId
        };

        if (request.Conditions.Any())
        {
            ChildConditionService customService = new(_uow);
            await updated.SetChildConditions(
                customService, request.Conditions, cancellationToken);
        }

        await _uow.Children.Update(updated, cancellationToken);
        await _uow.SaveChangesAsync();

        return new(updated);
    }
}
