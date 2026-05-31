using ABC.SharedKernel.Enums;

namespace ABC.Management.Domain.Validators;

public class EntityTranslationValidator : AbstractValidator<EntityTranslation>
{
    public EntityTranslationValidator()
    {
        RuleFor(x => x.EntityType).IsInEnum();
        RuleFor(x => x.EntityId).NotEmpty();
        RuleFor(x => x.Language).NotEmpty().MaximumLength(5);
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
    }
}
