using ABC.Management.Api.Commands;
using ABC.Management.Domain.Entities;
using ABC.SharedKernell;
using HotChocolate.Resolvers;
using Mediator;

namespace ABC.Management.Api.Types;

[MutationType]
public static class Mutation
{
    public static async Task<Antecedent?> AddAntecedent(
        IMediator handler,
        string name,
        string description,
        IResolverContext context)
    {
        var command = CreateAntecedentCommand.Create(name, description);
        return await ExecuteCommandHandler(handler, context, command);
    }

    public static async Task<Behavior?> AddBehavior(
        IMediator handler,
        string name,
        string description,
        IResolverContext context)
    {
        var command = CreateBehaviorCommand.Create(name, description);
        return await ExecuteCommandHandler(handler, context, command);
    }

    public static async Task<Consequence?> AddConsequence(
        IMediator handler,
        string name,
        string description,
        IResolverContext context)
    {
        var command = CreateConsequenceCommand.Create(name, description);
        return await ExecuteCommandHandler(handler, context, command);
    }

    public static async Task<Child?> AddChild(
        IMediator handler,
        string lastName,
        string firstName,
        int age,
        IResolverContext context)
    {
        var command = CreateChildCommand.Create(lastName, firstName, age);
        return await ExecuteCommandHandler(handler, context, command);
    }


    private static async Task<T?> ExecuteCommandHandler<T>(
        IMediator handler,
        IResolverContext context,
        IRequest<BaseResponseCommand<T>> command) where T : Entity
    {
        var response = await handler.Send(command);
        response.Errors.ForEach(context.ReportError);

        return response.Entity;
    }
}
