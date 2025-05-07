
using ABC.Management.Domain.Entities;
using ABC.SharedKernel;
using System.Collections;
using System.Threading;

namespace ABC.Management.Domain.Validators;

public class ChildValidator : AbstractValidator<Child>
{
    private readonly IEntityService<ChildCondition> _entityService;

    public ChildValidator(IEntityService<ChildCondition> entityService)
    {
        _entityService = entityService;

        var minYear = DateTime.UtcNow.Year - 100;
        var maxYear = DateTime.UtcNow.Year;

        RuleFor(x => x.LastName).NotEmpty();
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.BirthYear)
            .GreaterThanOrEqualTo(minYear)
            .LessThanOrEqualTo(maxYear);

        RuleFor(x => x.Conditions).MustAsync(InvalidateIfChildConditionNotFound)
           .WithMessage("some child conditions are not valid in the list")
           .WithErrorCode(nameof(InvalidateIfChildConditionNotFound));
    }

    private async Task<bool> InvalidateIfChildConditionNotFound(
        ICollection<ChildCondition> conditions,
        CancellationToken token)
    {
        Hashtable results = new();
        foreach (var condition in conditions)
        {
            var exists = await _entityService.GetByName(condition.Name, token);
            results[condition] = exists;
        }


        return results.Count == 0 || !results.ContainsValue(null);
    }
}
