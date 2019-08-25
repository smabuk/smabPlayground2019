using smabPlayground2019.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace smabPlayground2019.Server.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AccountsController : ControllerBase
	{
		private static UserModel LoggedOutUser = new UserModel { IsAuthenticated = false };

		private readonly UserManager<IdentityUser> _userManager;

		public AccountsController(UserManager<IdentityUser> userManager)
		{
			_userManager = userManager;
		}

		[HttpPost]
		public async Task<IActionResult> Post([FromBody]RegisterModel model)
		{
			var newUser = new IdentityUser { UserName = model.Email, Email = model.Email };

			var result = await _userManager.CreateAsync(newUser, model.Password);

			if (!result.Succeeded)
			{
				var errors = result.Errors.Select(x => x.Description);

				return BadRequest(new RegisterResult { Successful = false, Errors = errors });

			}

			return Ok(new RegisterResult { Successful = true });
		}
	}
}
