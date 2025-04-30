namespace ABC.Management.Domain.Validators;

public class ChildValidator : AbstractValidator<Child>
{
    public ChildValidator()
    {
        var minYear = DateTime.UtcNow.Year - 100;
        var maxYear = DateTime.UtcNow.Year;

        RuleFor(x => x.LastName).NotEmpty();
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.BirthYear)
            .GreaterThanOrEqualTo(minYear)
            .LessThanOrEqualTo(maxYear);
    }
}
