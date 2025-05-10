
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
    }


}
