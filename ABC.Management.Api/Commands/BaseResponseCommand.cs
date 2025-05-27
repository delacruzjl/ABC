using ABC.SharedKernel;

namespace ABC.Management.Api.Commands;

public record BaseResponseCommand<T> where T : Entity
{
    private readonly List<IError> _errors = [];
    public BaseResponseCommand(List<IError> errors, T? entity)
    {
        _errors = errors ?? [];
        Entity = entity;
    }

    public BaseResponseCommand(List<IError> errors) : this(errors, null)
    {

    }

    public BaseResponseCommand(T? entity) : this([], entity)
    {
    }

    public BaseResponseCommand() : this([])
    {

    }

    public IReadOnlyList<IError> Errors => _errors;

    public T? Entity { get; set; }

    public void AddErrors(params List<IError> errors)
    {
        if (errors == null || errors.Count == 0)
        {
            return;
        }

        _errors.AddRange(errors);
    }
}
