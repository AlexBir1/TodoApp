using Microsoft.AspNetCore.Identity;

namespace TodoAPI.DAL.Entities
{
    public class Account : IdentityUser
    {
        public ICollection<Collection> Collections { get; set; }
        public ICollection<Category> Categories { get; set; }
    }
}
