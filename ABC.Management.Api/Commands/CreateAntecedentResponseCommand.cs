﻿using ABC.Management.Domain.Entities;
using Mediator;

namespace ABC.Management.Api.Commands;

public record CreateAntecedentResponseCommand(Antecedent Value) 
    : IRequest<BaseResponseCommand<Antecedent>> {

    public static CreateAntecedentResponseCommand Create(string name, string description)
    {
        Antecedent antecedent = new()
        {
            Name = name,
            Description = description
        };

        return new(antecedent);
    }

}
