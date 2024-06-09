using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;
using NuGet.Protocol.Plugins;
using VoterServer.Data;
using VotingServer.Models;
using VotingServer.TempModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VotingServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoteController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public VoteController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Vote/5
        //Here {id} is project id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetVote(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            var inFavour = await _context.Votes.Where(e => e.ProjectId == id).Where(v => v.IsInFavor == true).CountAsync();
            var notInFavour = await _context.Votes.Where(e => e.ProjectId == id).Where(v => v.IsInFavor == false).CountAsync();
            return Ok(new { inFavour, notInFavour });
        }

		// POST: api/Vote
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		// {id} is project id
		[HttpPost("{id}")]
        [Authorize]
        public async Task<IActionResult> PostVote(int id, VoteDTO voteDto)
        {
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var project = await _context.Projects.FindAsync(id);
			if (project == null)
			{
				return NotFound();
			}
			var user = await _userManager.GetUserAsync(User);
			var userId = user.Id;

			if (user == null)
			{
				return Unauthorized();
			}
			var hasVoted = await _context.Votes.AnyAsync(p => p.ProjectId == id && p.UserId == userId);
            if (hasVoted) return Forbid();
			var vote = new Vote
			{
				UserId = userId,
	            ProjectId = id,
	            IsInFavor = voteDto.IsInFavor,
	            CreatedAt = DateTime.UtcNow,
			};
			_context.Votes.Add(vote);
            await _context.SaveChangesAsync();
            return Created();
        }
	}
}
