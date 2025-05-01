using ABC.Management.Api.Commands;
using ABC.Management.Api.Extensions;
using ABC.Management.Domain.Entities;
using HotChocolate.Resolvers;
using Mediator;

namespace ABC.Management.Api.Types;

[MutationType]
public static class Mutation
{
    [GraphQLDescription("Add a new antecedent")]
    public static async Task<Antecedent?> AddAntecedent(
        IMediator handler,
        string name,
        string description,
        IResolverContext context)
    {
        var command = CreateAntecedentResponseCommand.Create(name, description);
        return await command.ExecuteHandler(handler, context);
    }

    [GraphQLDescription("Add a new behavior")]
    public static async Task<Behavior?> AddBehavior(
        IMediator handler,
        string name,
        string description,
        IResolverContext context)
    {
        var command = CreateBehaviorResponseCommand.Create(name, description);
        return await command.ExecuteHandler(handler, context);
    }

    [GraphQLDescription("Add a new consequence")]
    public static async Task<Consequence?> AddConsequence(
        IMediator handler,
        string name,
        string description,
        IResolverContext context)
    {
        var command = CreateConsequenceResponseCommand.Create(name, description);
        return await command.ExecuteHandler(handler, context);
    }

    [GraphQLDescription("Add a new child")]
    public static async Task<Child?> AddChild(
        IMediator handler,
        string lastName,
        string firstName,
        int birthYear,
        IEnumerable<string>? conditions,
        IResolverContext context)
    {
        var command = CreateChildResponseCommand.Create(lastName, firstName, birthYear, conditions);
        return await command.ExecuteHandler(handler, context);
    }
}
