using ABC.Management.Api.Commands;
using ABC.Management.Domain.Entities;
using ABC.PostGreSQL.ValidationServices;
using ABC.SharedEntityFramework;
using Mediator;
using Microsoft.EntityFrameworkCore;

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

        var child = childQry
            .Include(c => c.Conditions)
            .Single();

        child.LastName = request.LastName;
        child.FirstName = request.FirstName;
        child.BirthYear = request.BirthYear;
        child.UserId = request.UserId;

        child.ClearConditions();
        if (request.Conditions.Any())
        {
            ChildConditionService customService = new(_uow);
            await child.SetChildConditions(
                customService, request.Conditions, cancellationToken);
        }

        await _uow.SaveChangesAsync();

        return new(child);
    }
}
