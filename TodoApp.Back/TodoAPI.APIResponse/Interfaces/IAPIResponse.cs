

namespace TodoAPI.APIResponse.Interfaces
{
    public interface IAPIResponse<T>
    {
        bool IsSuccess { get; }
        IEnumerable<string> Messages { get; }
    }
}
