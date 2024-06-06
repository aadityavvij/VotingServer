using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VotingServer.Models
{
	public class Vote
	{
		[Key]
		public int Id { get; set; }

		[ForeignKey("User")]
		public string UserId { get; set; }

		[ForeignKey("Project")]
		public int ProjectId { get; set; }

		[Required]
		public bool IsInFavor { get; set; }

		public DateTime CreatedAt { get; set; } = DateTime.Now;

		public virtual IdentityUser User { get; set; }
		public virtual Project Project { get; set; }
	}
}
