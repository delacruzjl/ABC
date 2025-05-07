using ABC.Management.Domain.Entities;
using ABC.SharedKernel;

namespace ABC.PostGreSQL;

public interface IUnitOfWork : IDisposable
{
    IRepository<Antecedent> Antecedents { get; }
    IRepository<Behavior> Behaviors { get; }
    IRepository<Child> Children { get; }
    IRepository<Consequence> Consequences { get; }
    IRepository<ChildCondition> ChildConditions { get; }

    Task<int> SaveChangesAsync();
}