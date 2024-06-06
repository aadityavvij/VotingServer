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
		[HttpGet("{id}")]
		public async Task<IActionResult> GetUserAdditional(string id)
		{
			var user = await _userManager.FindByIdAsync(id);
			if (user == null)
			{
				return NotFound();
			}
			return Ok(new {  user.UserName });
		}
	}
}
