using Microsoft.AspNetCore.Identity;

namespace HRV.Data.Db
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser() 
        {
        
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class ApplicationRole : IdentityRole
    {

    }
}
