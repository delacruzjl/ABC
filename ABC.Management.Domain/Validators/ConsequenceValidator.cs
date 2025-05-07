namespace ABC.Management.Domain.Validators;

[ExcludeFromCodeCoverage]
public class ConsequenceValidator : AbstractValidator<Consequence>
{
    private readonly IEntityService<Consequence> _service;

    public ConsequenceValidator(IEntityService<Consequence> service)
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();

        _service = service;
        RuleFor(x => x).MustAsync(InvalidateIfNameAlreadyExists)
            .WithMessage("An entity with this name already exists")
            .WithErrorCode(nameof(InvalidateIfNameAlreadyExists));
    }

    private async Task<bool> InvalidateIfNameAlreadyExists(
    Consequence entity,
    CancellationToken cancellationToken = default)
    {
        var exists = await _service.GetByName(entity.Name, cancellationToken);
        return exists == null || exists.Id == entity.Id;
    }
}