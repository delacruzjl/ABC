namespace ABC.Management.Domain.Validators;

public class ChildValidator : AbstractValidator<Child>
{
    public ChildValidator()
    {
        RuleFor(x => x.LastName).NotEmpty();
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.Age)
            .GreaterThan(0)
            .LessThan(21);
    }
}
