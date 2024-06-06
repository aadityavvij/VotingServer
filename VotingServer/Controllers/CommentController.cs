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
    public class CommentController : ControllerBase
    {
		private readonly ApplicationDbContext _context;
		private readonly UserManager<IdentityUser> _userManager;

		public CommentController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

		// GET: api/Comment/5
		//Here {id} is project id
		[HttpGet("{id}")]
		public async Task<IActionResult> GetComment(int id)
		{
			var project = await _context.Projects.FindAsync(id);
			if (project == null)
			{
				return NotFound();
			}
			var comments = await _context.Comments.Where(e => e.ProjectId == id).ToListAsync();

			return Ok(comments);
		}

		// PUT: api/Comment/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		// {id} is comment id
		[HttpPut("{id}")]
		[Authorize]
        public async Task<IActionResult> PutComment(int id, CommentDTO commentDto)
        {
			var comment = await _context.Comments.FindAsync(id);
			if (comment == null)
			{
				return NotFound();
			}
			var user = await _userManager.GetUserAsync(User);
			var userId = user.Id;
			if (user == null || user.Id != comment.UserId)
			{
				return Unauthorized();
			}

			comment.Content = commentDto.Content;

			_context.Comments.Update(comment);
			await _context.SaveChangesAsync();

			return NoContent();
        }

		// POST: api/Comment
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		// {id} is project id
		[HttpPost("{id}")]
		[Authorize]
		public async Task<IActionResult> PostComment(int id, CommentDTO commentDto)
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
			var comment = new Comment
			{
				UserId = userId,
				ProjectId = id,
				Content = commentDto.Content,
				CreatedAt = DateTime.UtcNow,
			};
			_context.Comments.Add(comment);
			await _context.SaveChangesAsync();
			return Created();
		}

		// DELETE: api/Comment/5
		// {id} is comment id
		[HttpDelete("{id}")]
		[Authorize]
        public async Task<IActionResult> DeleteComment(int id)
        {
			var comment = await _context.Comments.FindAsync(id);
			if (comment == null)
			{
				return NotFound();
			}
			var user = await _userManager.GetUserAsync(User);
			var userId = user.Id;
			if (user == null || user.Id != comment.UserId)
			{
				return Unauthorized();
			}

			_context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
