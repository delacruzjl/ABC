namespace ABC.Management.Domain.Validators;

public class ChildConditionValidator : AbstractValidator<ChildCondition>
{
    private readonly IEntityService<ChildCondition> _entityService;

    public ChildConditionValidator(IEntityService<ChildCondition> entityService)
    {
        _entityService = entityService;
        RuleFor(x => x.Name).NotEmpty();

        RuleFor(x => x).MustAsync(InvalidateIfNameAlreadyExists)
           .WithMessage("An entity with this name already exists")
           .WithErrorCode("DuplicateNameValidator");
    }

    private async Task<bool> InvalidateIfNameAlreadyExists(
       ChildCondition antecedent,
       CancellationToken cancellationToken = default)
    {
        var exists = await _entityService.GetByName(antecedent.Name, cancellationToken);
        return exists == null || exists.Id == antecedent.Id;
    }
}
