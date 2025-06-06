﻿using ABC.Management.Api.Commands;
using ABC.Management.Domain.Entities;
using ABC.SharedEntityFramework;
using Mediator;

namespace ABC.Management.Api.Handlers;

public class CreateConsequenceResponseHandler(IUnitOfWork _uow)
    : IRequestHandler<CreateConsequenceResponseCommand, BaseResponseCommand<Consequence>>
{
    public async ValueTask<BaseResponseCommand<Consequence>> Handle(
        CreateConsequenceResponseCommand request,
        CancellationToken cancellationToken)
    {
        BaseResponseCommand<Consequence> response = new()
        {
            Entity = await _uow.Consequences.AddAsync(request.Value, cancellationToken)
        };

        var count = await _uow.SaveChangesAsync();
        if (count == 0)
        {
            throw new InvalidOperationException("Nothing saved to database");
        }

        return response;
    }
}