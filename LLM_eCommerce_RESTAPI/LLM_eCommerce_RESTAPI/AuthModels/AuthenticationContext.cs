using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LLM_eCommerce_RESTAPI.AuthModels
{
    public class AuthenticationContext : IdentityDbContext
    {
        public AuthenticationContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var roleId_1 = Guid.NewGuid().ToString();
            var userId_1 = Guid.NewGuid().ToString();

            var roleId_2 = Guid.NewGuid().ToString();
            var userId_2 = Guid.NewGuid().ToString();

            var roleId_3 = Guid.NewGuid().ToString();
            var userId_3 = Guid.NewGuid().ToString();

            #region "Seed Data"
            builder.Entity<IdentityRole>().HasData(
                new { Id = roleId_1, Name = "Administrator", NormalizedName = "ADMINISTRATOR" },
                new { Id = roleId_2, Name = "Seller", NormalizedName = "SELLER" },
                new { Id = roleId_3, Name = "Customer", NormalizedName = "CUSTOMER" }
                );




            //create Administrator user
            var AdminUser = new ApplicationUser
            {
                Id = userId_1,
                Email = "Zzimela@gmail.com",
                EmailConfirmed = true,
                Address = "18 Jack avenue, 2001",
                PhoneNumber = "0743244345",
                FirstName = "Zandile",
                LastName = "Zimela",
                UserName = "ZzimelaAdmin",
                NormalizedUserName = "ZZIMELAADMIN"
            };

            //set user password
            PasswordHasher<ApplicationUser> ph = new PasswordHasher<ApplicationUser>();
            AdminUser.PasswordHash = ph.HashPassword(AdminUser, "zimelaZ@1234");

            //seed user
            builder.Entity<ApplicationUser>().HasData(AdminUser);

            //set user role to Administrator
            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = roleId_1,
                UserId = userId_1
            });

            //create Seller user
            var SellerUser = new ApplicationUser
            {
                Id = userId_2,
                Email = "cfarquarson0@cam.ac.uk",
                EmailConfirmed = true,
                Address = "25629 Fulton Pass",
                PhoneNumber = "0546292527",
                FirstName = "Corliss",
                LastName = "Farquarson",
                UserName = "cfarquarson0",
                NormalizedUserName = "CFARQUARSON0"
            };

            //set user password
            PasswordHasher<ApplicationUser> cstph = new PasswordHasher<ApplicationUser>();
            SellerUser.PasswordHash = cstph.HashPassword(SellerUser, "corlissF@123");

            //seed user
            builder.Entity<ApplicationUser>().HasData(SellerUser);

            //set user role to admin
            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = roleId_2,
                UserId = userId_2
            });
            
            //create Customer user
            var CustomerUser = new ApplicationUser
            {
                Id = userId_3,
                Email = "Efronz@gmail.com",
                EmailConfirmed = true,
                Address = "15 Zuma avenue, 2001",
                PhoneNumber = "074396748",
                FirstName = "Zack",
                LastName = "Efron",
                UserName = "Efronz",
                NormalizedUserName = "EFRONZ"
            };

            //set user password
            PasswordHasher<ApplicationUser> lgcph = new PasswordHasher<ApplicationUser>();
            CustomerUser.PasswordHash = lgcph.HashPassword(CustomerUser, "Efron@123456");

            //seed user
            builder.Entity<ApplicationUser>().HasData(CustomerUser);

            //set user role to admin
            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = roleId_3,
                UserId = userId_3
            });



            #endregion

        }
    }
}
