using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LLM_eCommerce_RESTAPI.AuthModels
{
    public class ApplicationUser : IdentityUser
    {
        [StringLength(50), Column("first_name")]
        public string? FirstName { get; set; }

        [StringLength(50), Column("last_name")]
        public string? LastName { get; set; }

        [StringLength(255), Column("address")]
        public string? Address { get; set; }

    }
}
