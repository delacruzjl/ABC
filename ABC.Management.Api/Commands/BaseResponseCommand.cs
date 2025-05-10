using ABC.SharedKernel;

namespace ABC.Management.Api.Commands;

public record BaseResponseCommand<T>  where T : Entity
{
    public BaseResponseCommand(List<IError> errors, T? entity)
    {
        Errors = errors ?? [];
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

    public List<IError> Errors { get; set; }

    public T? Entity { get; set; }
}
