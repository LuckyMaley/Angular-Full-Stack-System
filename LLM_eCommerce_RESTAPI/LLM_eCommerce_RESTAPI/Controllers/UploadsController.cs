using LLM_eCommerce_RESTAPI.AuthModels;
using LLM_eCommerce_RESTAPI.Models;
using LLM_eCommerce_RESTAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LLM_eCommerce_RESTAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UploadsController : ControllerBase
	{
		private readonly IWebHostEnvironment _environment;
		private readonly LLM_eCommerce_EFDBContext _context;
		private UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly AuthenticationContext _authenticationContext;
		private readonly IdentityHelper _identityHelper;
		private readonly string _uploadPath;

		public UploadsController(IWebHostEnvironment environment, LLM_eCommerce_EFDBContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, AuthenticationContext authenticationContext)
		{
			_context = context;
			_userManager = userManager;
			_roleManager = roleManager;
			_authenticationContext = authenticationContext;
			_identityHelper = new IdentityHelper(_userManager, _authenticationContext, _roleManager);
			_environment = environment;

			_uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
			if (!Directory.Exists(_uploadPath))
			{
				Directory.CreateDirectory(_uploadPath);
			}
		}


		// GET: api/Upload        
		[EnableCors("AllowOrigin")]
		[HttpPost]
		[Authorize]
		public async Task<IActionResult> Upload([FromForm] IFormFile file)
		{
			string userId = User.Claims.First(c => c.Type == "UserID").Value;
			var user = await _userManager.FindByIdAsync(userId);
			bool userSuperUserAuthorised = await _identityHelper.IsSuperUserRole(userId);
			bool userSellerAuthorised = await _identityHelper.IsSellerUserRole(userId);
			if (!userSuperUserAuthorised && !userSellerAuthorised)
			{
				return BadRequest(new { message = "Not authorised to update products as a customer" });
			}

			if (file == null || file.Length == 0)
				return BadRequest("No file uploaded.");

			var filePath = Path.Combine(_uploadPath, file.FileName);

			using (var fileStream = new FileStream(filePath, FileMode.Create))
			{
				await file.CopyToAsync(fileStream);
			}

			var fileUrl = Url.Action("GetFile", new { fileName = file.FileName });

			return Ok(new { imageUrl = fileUrl });
		}

		[HttpGet("{fileName}")]
		public IActionResult GetFile(string fileName)
		{
			var filePath = Path.Combine(_uploadPath, fileName);

			if (!System.IO.File.Exists(filePath))
				return NotFound();

			var fileBytes = System.IO.File.ReadAllBytes(filePath);
			return File(fileBytes, "application/octet-stream", fileName);
		}
	}
}
