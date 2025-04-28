using ABC.Management.Domain.Entities;

namespace ABC.Management.Domain.Validators;

[ExcludeFromCodeCoverage]
public class BehaviorValidator : AbstractValidator<Behavior>
{
    private readonly IEntityService<Behavior> _service;

    public BehaviorValidator(IEntityService<Behavior> service)
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
        _service = service;

        RuleFor(x => x).MustAsync(InvalidateIfNameAlreadyExists)
            .WithMessage("An entity with this name already exists")
            .WithErrorCode("DuplicateNameValidator");
    }

    private async Task<bool> InvalidateIfNameAlreadyExists(
        Behavior entity,
        CancellationToken cancellationToken = default)
    {
        var exists = await _service.GetByName(entity.Name, cancellationToken);
        return exists == null || exists.Id == entity.Id;
    }
}
