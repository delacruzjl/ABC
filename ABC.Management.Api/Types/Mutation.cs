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
        var command = CreateAntecedentCommand.Create(name, description);
        return await command.ExecuteHandler(handler, context);
    }

    [GraphQLDescription("Add a new behavior")]
    public static async Task<Behavior?> AddBehavior(
        IMediator handler,
        string name,
        string description,
        IResolverContext context)
    {
        var command = CreateBehaviorCommand.Create(name, description);
        return await command.ExecuteHandler(handler, context);
    }

    [GraphQLDescription("Add a new consequence")]
    public static async Task<Consequence?> AddConsequence(
        IMediator handler,
        string name,
        string description,
        IResolverContext context)
    {
        var command = CreateConsequenceCommand.Create(name, description);
        return await command.ExecuteHandler(handler, context);
    }

    [GraphQLDescription("Add a new child")]
    public static async Task<Child?> AddChild(
        IMediator handler,
        string lastName,
        string firstName,
        int age,
        IResolverContext context)
    {
        var command = CreateChildCommand.Create(lastName, firstName, age);
        return await command.ExecuteHandler(handler, context);
    }
}
