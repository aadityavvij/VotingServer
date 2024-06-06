using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
    public class ProjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
		private readonly UserManager<IdentityUser> _userManager;

		public ProjectController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

		// GET: api/Project
		[HttpGet]
        public async Task<IActionResult> GetProjects()
        {
            return Ok(await _context.Projects.ToListAsync());
        }

        // GET: api/Project/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);

            if (project == null)
            {
                return NotFound();
            }

            return Ok(project);
        }

		// PUT: api/Project/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("{id}")]
		[Authorize]
		public async Task<IActionResult> PutProject(int id, ProjectDTO projectDto)
		{
			var project = await _context.Projects.FindAsync(id);
			if (project == null)
			{
				return NotFound();
			}

			var user = await _userManager.GetUserAsync(User);
			if (user == null || user.Id != project.CreatedById)
			{
				return Unauthorized();
			}

			project.Title = projectDto.Title;
			project.Description = projectDto.Description;
			project.Location = projectDto.Location;

			_context.Projects.Update(project);
			await _context.SaveChangesAsync();

			return NoContent();
		}


		// POST: api/Project
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
		[Authorize]
        public async Task<IActionResult> PostProject(ProjectDTO projectDto)
        {
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}


			var user = await _userManager.GetUserAsync(User);
			var userId = user.Id;

			if (user == null)
			{
				return Unauthorized();
			}
			var project = new Project
			{
				Title = projectDto.Title,
				Description = projectDto.Description,
				Location = projectDto.Location,
				Votes = 0,
				CreatedById = user.Id,
				CreatedAt = DateTime.UtcNow
			};
			_context.Projects.Add(project);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetProject", new { id = project.Id }, project);
        }

        // DELETE: api/Project/5
        [HttpDelete("{id}")]
		[Authorize]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

			var user = await _userManager.GetUserAsync(User);
			if (user == null || user.Id != project.CreatedById)
			{
				return Unauthorized();
			}

			_context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
