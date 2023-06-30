using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Models.DomainModels
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class EreborUser : IdentityUser
    {
        public string Name { get; set; } = "";
    }
}
