namespace ABC.Management.Domain.Validators;

public class AntecedentValidator : AbstractValidator<Antecedent>
{
    private readonly IEntityService<Antecedent> _antecedentService;

    public AntecedentValidator(IEntityService<Antecedent> antecedentService)
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
        
        _antecedentService = antecedentService;
        RuleFor(x => x).MustAsync(InvalidateIfNameAlreadyExists)
            .WithMessage("An entity with this name already exists")
            .WithErrorCode(nameof(InvalidateIfNameAlreadyExists));
    }

    private async Task<bool> InvalidateIfNameAlreadyExists(
        Antecedent antecedent,
        CancellationToken cancellationToken = default)
    {
        var exists = await _antecedentService.GetByName(antecedent.Name, cancellationToken);
        return exists == null || exists.Id == antecedent.Id;
    }
}
