using Microsoft.AspNetCore.Identity;

namespace LLM_eCommerce_RESTAPI.ViewModels
{
    public class UserAccountVM
    {
        public UserAccountVM()
        {
        }

        public UserAccountVM(IdentityUser aus, List<string> userRoles)
        {
            UserName = aus.UserName;
            Email = aus.Email;
            RolesHeld = userRoles;
        }

        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public List<string> RolesHeld { get; set; }

    }

}
