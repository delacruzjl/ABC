namespace ABC.Management.Domain.Entities;
public class Child(
    Guid id,
    string lastName,
    string firstName,
    int age) : Entity(id)
{
    public string LastName { get; init; } = lastName;
    public string FirstName { get; init; } = firstName;
    public int Age { get; init; } = age;


    public Child() : this(Guid.NewGuid(), string.Empty, string.Empty, 0)
    {

    }
}
