using LLM_eCommerce_RESTAPI.AuthModels;
using LLM_eCommerce_RESTAPI.Models;
using LLM_eCommerce_RESTAPI.Services;
using LLM_eCommerce_RESTAPI.ViewModels;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LLM_eCommerce_RESTAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    
    public class UserProfileController : ControllerBase
    {
        private static readonly ILog logger = LogManager.GetLogger("ApplicationUserController");
        private readonly IdentityHelper _identityHelper;

        private UserManager<ApplicationUser> _userManager;
        private readonly AuthenticationContext _context;
        private readonly LLM_eCommerce_EFDBContext _contextEFDB;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserProfileController(UserManager<ApplicationUser> userManager,
            AuthenticationContext context, RoleManager<IdentityRole> roleManager, LLM_eCommerce_EFDBContext contextEFDB)
        {
            _userManager = userManager;
            _context = context;
            _contextEFDB = contextEFDB;
            _roleManager = roleManager;
            _identityHelper = new IdentityHelper(userManager, _context, roleManager);
        }



        [EnableCors("AllowOrigin")]
        [HttpGet]
		[Authorize]
		// Get : /api/UserProfile
		public async Task<Object> Get()
        {
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);


            List<string> userRoles = new List<string>(await _userManager.GetRolesAsync(user));

            return new
            {
                user.FirstName,
                user.LastName,
                user.Email,
                user.UserName,
                user.Address,
                user.PhoneNumber,
                userRoles
            };
        }

        [EnableCors("AllowOrigin")]
        [HttpPut]
        [Authorize]
        // Put : /api/UserProfile
        public async Task<IActionResult> UpdateUserProfile([FromBody] UpdateUserProfileVM model)
        {
            try
            {
                string userId = User.Claims.First(c => c.Type == "UserID").Value;
                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    logger.Error("User not found.");
                    return NotFound("User not found.");
                }

                if (!string.IsNullOrEmpty(model.FirstName))
                {
                    user.FirstName = model.FirstName;
                }
                if (!string.IsNullOrEmpty(model.LastName))
                {
                    user.LastName = model.LastName;
                }
                if (!string.IsNullOrEmpty(model.Email))
                {
                    user.Email = model.Email;
                }
                if (!string.IsNullOrEmpty(model.Address))
                {
                    user.Address = model.Address;
                }
                if (!string.IsNullOrEmpty(model.PhoneNumber))
                {
                    user.PhoneNumber = model.PhoneNumber;
                }
   
                var usrEFDB = _contextEFDB.EfUsers.FirstOrDefault(c => c.IdentityUsername == user.UserName);
                if (_contextEFDB.EfUsers.Where(c => c.IdentityUsername == user.UserName).Count() == 0)
                {
                    logger.Error("ERROR Updating user: user does not exist.");
                    return BadRequest(new { message = "ERROR Updating user: user does not exist." });
                }

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    usrEFDB.Email = user.Email;
                    usrEFDB.Address = user.Address;
                    usrEFDB.PhoneNumber = user.PhoneNumber;
                    usrEFDB.FirstName = user.FirstName;
                    usrEFDB.LastName = user.LastName;
                    _contextEFDB.SaveChanges();
                    logger.Info("User profile updated successfully.");
                    return Ok(new
                    {
                        user.FirstName,
                        user.LastName,
                        user.Email,
                        user.UserName,
                        user.Address,
                        user.PhoneNumber
                    });
                }
                else
                {
                    logger.Error("Error updating user profile: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                    return BadRequest(result.Errors);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error in UpdateUserProfile method", ex);
                return StatusCode(500, "Internal server error");
            }
        }



        [EnableCors("AllowOrigin")]
        [HttpPut]
        [Route("Admin")]
        [Authorize]
        // Put : /api/UserProfile
        public async Task<IActionResult> AdminUpdateUserProfile([FromBody] AdminUpdateUserProfileVM model)
        {
            try
            {
                string userId = User.Claims.First(c => c.Type == "UserID").Value;
                var user = await _userManager.FindByNameAsync(model.UserName);

                if (user == null)
                {
                    logger.Error("User not found.");
                    return NotFound("User not found.");
                }

                if (!string.IsNullOrEmpty(model.FirstName))
                {
                    user.FirstName = model.FirstName;
                }
                if (!string.IsNullOrEmpty(model.LastName))
                {
                    user.LastName = model.LastName;
                }
                if (!string.IsNullOrEmpty(model.Email))
                {
                    user.Email = model.Email;
                }
                if (!string.IsNullOrEmpty(model.Address))
                {
                    user.Address = model.Address;
                }
                if (!string.IsNullOrEmpty(model.PhoneNumber))
                {
                    user.PhoneNumber = model.PhoneNumber;
                }

                var usrEFDB = _contextEFDB.EfUsers.FirstOrDefault(c => c.IdentityUsername == user.UserName);
                if (_contextEFDB.EfUsers.Where(c => c.IdentityUsername == user.UserName).Count() == 0)
                {
                    logger.Error("ERROR Updating user: user does not exist.");
                    return BadRequest(new { message = "ERROR Updating user: user does not exist." });
                }


                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    usrEFDB.Email = user.Email;
                    usrEFDB.Address = user.Address;
                    usrEFDB.PhoneNumber = user.PhoneNumber;
                    usrEFDB.FirstName = user.FirstName;
                    usrEFDB.LastName = user.LastName;
                    _contextEFDB.SaveChanges();
                    if (!string.IsNullOrEmpty(model.Role))
                    {
                        var currentRoles = await _userManager.GetRolesAsync(user);
                        var removeRolesResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
                        if (!removeRolesResult.Succeeded)
                        {
                            logger.Error("Error removing user roles: " + string.Join(", ", removeRolesResult.Errors.Select(e => e.Description)));
                            return BadRequest(removeRolesResult.Errors);
                        }

                        var addRoleResult = await _userManager.AddToRoleAsync(user, model.Role);
                        if (!addRoleResult.Succeeded)
                        {
                            logger.Error("Error adding user role: " + string.Join(", ", addRoleResult.Errors.Select(e => e.Description)));
                            return BadRequest(addRoleResult.Errors);
                        }
                    }

                    logger.Info("User profile updated successfully.");
                    return Ok(new
                    {
                        user.FirstName,
                        user.LastName,
                        user.Email,
                        user.UserName,
                        user.Address,
                        user.PhoneNumber,
                        rolesHeld = GetUserRoles(user).Result
                    });
                }
                else
                {
                    logger.Error("Error updating user profile: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                    return BadRequest(result.Errors);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error in UpdateUserProfile method", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: ApplicationUserRoles
        [EnableCors("AllowOrigin")]
        [HttpGet("specificUser/{username}")]
		[Authorize]
		// Get : /api/UserProfile/specificUser/Details
		// FromBody
		public async Task<Object> GetUserList([FromRoute] string username)
        {
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            bool userAuthorised = await _identityHelper.IsSuperUserRole(userId);


            if (user != null && userAuthorised)
            {
                var usr = await _userManager.FindByNameAsync(username);

                if (usr == null)
                {
                    return NotFound(new { message = "Username not found" });
                }
                else
                {
                    return new
                    {
                        usr.FirstName,
                        usr.LastName,
                        usr.Email,
                        usr.UserName,
                        usr.Address,
                        usr.PhoneNumber,
                        RolesHeld = GetUserRoles(usr).Result
                    };
                }
            }
            else
            {
                return BadRequest(new { message = "Invalid Username or Role." });
            }

        }

		// GET: ApplicationUserRoles
		[EnableCors("AllowOrigin")]
		[HttpGet("UserRegCheck/{username}")]
		// Get : /api/UserProfile/UserRegCheck/{username}
		// FromBody
		public async Task<Object> GetUserRegCheckList(string username)
		{
				var usr = await _userManager.FindByNameAsync(username);

				if (usr == null)
				{
					return NotFound(new { message = "Username not found" });
				}
				else
				{
					return new
					{
						usr.FirstName,
						usr.LastName,
						usr.Email,
						usr.UserName,
						usr.Address,
						usr.PhoneNumber,
						RolesHeld = GetUserRoles(usr).Result
					};
				}

		}

		[EnableCors("AllowOrigin")]
        [HttpDelete("DeleteUser/{username}")]
        // Delete : /api/UserProfile/DeleteUser/Details
        [Authorize]
        public async Task<IActionResult> DeleteUser([FromRoute] string username)
        {
            try
            {
                string userId = User.Claims.First(c => c.Type == "UserID").Value;
                var user = await _userManager.FindByIdAsync(userId);

                bool userAuthorised = await _identityHelper.IsSuperUserRole(userId);

                if (user != null && userAuthorised)
                {
                    var usr = await _userManager.FindByNameAsync(username);

                    if (usr == null)
                    {
                        return NotFound(new { message = "Username not found" });
                    }
                    else
                    {
                        var result = await _userManager.DeleteAsync(usr);
                        if (result.Succeeded)
                        {
                            var usrEFDB = _contextEFDB.EfUsers.FirstOrDefault(c => c.IdentityUsername == usr.UserName);
                            if (usrEFDB != null)
                            {
                                _contextEFDB.EfUsers.Remove(usrEFDB);
                                _contextEFDB.SaveChanges();
                            }

                            logger.Info($"User {username} deleted successfully.");
                            return Ok(new { message = "User deleted successfully" });
                        }
                        else
                        {
                            logger.Error($"Error deleting user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                            return BadRequest(result.Errors);
                        }
                    }
                }
                else
                {
                    return BadRequest(new { message = "Invalid Username or Role." });
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error in DeleteUser method", ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [EnableCors("AllowOrigin")]
        [Route("AllUsers")]
        // Get : /api/UserProfile/AllUsers
        [HttpGet]
        [Authorize]
        public async Task<Object> GetUserList()
        {

            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            bool userAuthorised = await _identityHelper.IsSuperUserRole(userId);

            List<UserAccountVM> allUsersWithRoles = new List<UserAccountVM>();

            if (user != null && userAuthorised)
            {
                var userList = (from usr in await _userManager.Users.ToListAsync()
                                select new
                                {
                                    usr.UserName,
                                    usr.Email,
                                    usr.FirstName,
                                    usr.LastName,
                                    usr.Address,
                                    usr.PhoneNumber,
                                    RolesHeld = new List<string>()
                                }).ToList();


                foreach (var userListVm in userList)
                {
                    var details = await _userManager.FindByNameAsync(userListVm.UserName);

                    allUsersWithRoles.Add(new UserAccountVM()
                    {
                        UserName = userListVm.UserName,
                        Email = userListVm.Email,
                        FirstName = details.FirstName,
                        LastName = details.LastName,
                        Address = details.Address,
                        PhoneNumber = details.PhoneNumber,
                        RolesHeld = GetUserRoles(details).Result
					});
                }

                return allUsersWithRoles.ToList();
            }
            else
            {
                return BadRequest(new { message = "Invalid Username or Role." });
            }
        }

        private async Task<List<string>> GetUserRoles(IdentityUser selectedUser)
        {
            var usr = await _userManager.FindByNameAsync(selectedUser.UserName);

            return new List<string>(await _userManager.GetRolesAsync(usr));
        }

    }
}
