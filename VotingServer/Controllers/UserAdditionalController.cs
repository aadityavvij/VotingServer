using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VoterServer.Data;
using VotingServer.Models;
using VotingServer.TempModels;

namespace VotingServer.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserAdditionalController : ControllerBase
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<IdentityUser> _userManager;

		public UserAdditionalController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}
		[Authorize]
		[HttpGet]
		public async Task<IActionResult> GetUserAdditional()
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				return NotFound();
			}
			UserAdditional u = new UserAdditional();
			u.Id = user.Id;
			u.UserName = user.UserName;
			u.Email = user.Email;
			return Ok(u);
		}
	}
}
