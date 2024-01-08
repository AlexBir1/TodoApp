
namespace TodoAPI.Shared.Models
{
    public class TokenModel
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ValidTo { get; set; }
    }
}
