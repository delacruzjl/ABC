using ABC.Management.Api.Commands;
using ABC.Management.Domain.Entities;
using ABC.PostGreSQL;
using FluentValidation;
using Mediator;

namespace ABC.Management.Api.Handlers;

public class CreateChildResponseHandler(IUnitOfWork _uow)
    : IRequestHandler<CreateChildResponseCommand, BaseResponseCommand<Child>>
{
    public async ValueTask<BaseResponseCommand<Child>> Handle(
        CreateChildResponseCommand request,
        CancellationToken cancellationToken)
    {
        BaseResponseCommand<Child> response = new()
        { 
            Entity = await _uow.Children.AddAsync(request.Value, cancellationToken)
        };

        var count = await _uow.SaveChangesAsync();
        if (count == 0)
        {
            throw new InvalidOperationException("Nothing saved to database");
        }

        return response;
    }
}
