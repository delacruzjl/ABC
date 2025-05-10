using ABC.Management.Api.Commands;
using ABC.Management.Domain.Entities;
using ABC.PostGreSQL.ValidationServices;
using ABC.SharedEntityFramework;
using ABC.SharedKernel;
using Mediator;
using System.Collections;

namespace ABC.Management.Api.Handlers;

public class CreateChildResponseHandler(
    IUnitOfWork _uow)
    : IRequestHandler<CreateChildResponseCommand, BaseResponseCommand<Child>>
{
    public async ValueTask<BaseResponseCommand<Child>> Handle(
        CreateChildResponseCommand request,
        CancellationToken cancellationToken)
    {
        Child entity = new(
            request.Value.Id,
            request.Value.LastName,
            request.Value.FirstName,
            request.Value.BirthYear);

        ChildConditionService customService = new(_uow);
        await entity.SetChildConditions(
            customService, request.Conditions, cancellationToken);
        await _uow.Children.AddAsync(entity, cancellationToken);

        await _uow.SaveChangesAsync();
        return new(entity);
    }
}
