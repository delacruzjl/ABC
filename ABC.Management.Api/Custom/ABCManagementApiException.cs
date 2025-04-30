namespace ABC.Management.Api.Custom;

public class ABCManagementApiException : Exception
{
    public List<IError> Errors { get; private set; }

    public ABCManagementApiException()
    {
        Errors = new List<IError>();
    }

    public ABCManagementApiException(string message)
        : base(message)
    {
        Errors = new List<IError>();
    }

    public ABCManagementApiException(string message, Exception inner, List<IError> errors)
        : base(message, inner)
    {
        Errors = errors;
    }
}
