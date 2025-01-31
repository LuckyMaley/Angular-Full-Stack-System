using LLM_eCommerce_RESTAPI.AuthModels;
using LLM_eCommerce_RESTAPI.Models;
using log4net;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LLM_eCommerce_RESTAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUserController : ControllerBase
    {
        private static readonly ILog logger = LogManager.GetLogger("ApplicationUserController");

        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationSettings _appSettings;
        private readonly LLM_eCommerce_EFDBContext _context;
        private readonly AuthenticationContext _authenticationContext;

        public ApplicationUserController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, IOptions<ApplicationSettings> appSettings, LLM_eCommerce_EFDBContext context, AuthenticationContext authenticationContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _appSettings = appSettings.Value;
            _context = context;
            _authenticationContext = authenticationContext;
        }


        [EnableCors("AllowOrigin")]
        [HttpPost]
        [Route("Register")]
        // Post : /api/ApplicationUser/Register
        public async Task<Object> PostApplicationUser(ApplicationUserModel model)
        {
            logger.Info("ApplicationUserController - Post : /api/ApplicationUser/Register");
            logger.Info("ApplicationUserController - Post : /api/ApplicationUser/Register model:" + model.LastName);

            var applicationUser = new ApplicationUser()
            {
                UserName = model.UserName,
                Email = model.Email,
                NormalizedEmail = model.Email.ToUpper(),
                NormalizedUserName = model.UserName.ToUpper(),
                Address = model.Address,
                PhoneNumber = model.PhoneNumber,                
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            if (model.Role == null)
            {  //Set default Role
                model.Role = "Customer";
            }

            try
            {

                var usrEFDB = new EfUser()
                {
                    IdentityUsername = model.UserName,
                    Email = model.Email,
                    Address = model.Address,
                    PhoneNumber = model.PhoneNumber,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Role = model.Role
                };

                if (_context.EfUsers.Where(c => c.IdentityUsername == model.UserName).Count() == 0)
                {
                    _context.EfUsers.Add(usrEFDB);
                }
                else
                {
                    return BadRequest(new { message = "ERROR Creating user: user already exists." });
                }

                var result = await _userManager.CreateAsync(applicationUser, model.Password);

                if (result.Succeeded)
                {
                    var userResult = await _userManager.AddToRoleAsync(applicationUser, model.Role);
                    await _context.SaveChangesAsync();
                    await _authenticationContext.SaveChangesAsync();

                }
                else
                {
                    return BadRequest(new { message = "ERROR Creating user: please make sure password has a Digit, Lowercase letter, Uppercase letter, NonAlphanumeric symbol, and Length of 12 characters;" });
                }
                return Ok(new { Username = applicationUser.UserName });
            }
            catch (Exception e)
            {

                return BadRequest(new { message = "ERROR Creating user: Username or password not VALID." + e });
            }
        }




        [EnableCors("AllowOrigin")]
        [HttpPost]
        [Route("Login")]
        // Post : /api/ApplicationUser/Login
        public async Task<IActionResult> Login(LoginModel model)
        {

            logger.Info("ApplicationUserController - Post : /api/ApplicationUser/Login");
            logger.Info("ApplicationUserController - Post : /api/ApplicationUser/Login model.UserName:" + model.UserName);

            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {

                var claim = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("UserID", user.Id.ToString())
                };


                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.SigningKey));
                string tmpKeyIssuer = _appSettings.JWT_Site_URL;
                int expiryInMinutes = Convert.ToInt32(_appSettings.ExpiryInMinutes);


                var usrToken = new JwtSecurityToken(
                    claims: claim,
                    expires: DateTime.Now.AddMinutes(expiryInMinutes),
                    signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                    );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(usrToken),
                    expiration = usrToken.ValidTo,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserName = user.UserName,
                    roles = await _userManager.GetRolesAsync(user)
                });

            }
            else
            {
                logger.Info("ApplicationUserController - Post : /api/ApplicationUser/Login - BadRequest:");

                return BadRequest(new { message = "Username or password not found." });
            }

        }
    }
}
