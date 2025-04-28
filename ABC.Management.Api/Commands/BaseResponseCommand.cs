using ABC.SharedKernel;

namespace ABC.Management.Api.Commands
{
    public class BaseResponseCommand<T>(List<IError> errors)  where T : Entity
    {
        public T? Entity { get; set; }
        public List<IError> Errors { get; set; } = errors;

        public BaseResponseCommand() : this([])
        {
            
        }
    }
}
