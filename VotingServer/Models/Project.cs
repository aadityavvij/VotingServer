using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VotingServer.Models
{
	public class Project
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[MaxLength(255)]
		public string Title { get; set; }

		[Required]
		public string Description { get; set; }

		[MaxLength(255)]
		public string Location { get; set; }

		public int Votes { get; set; } = 0;

		[ForeignKey("CreatedBy")]
		public string CreatedById { get; set; }

		public DateTime CreatedAt { get; set; } = DateTime.Now;

		public virtual IdentityUser CreatedBy { get; set; }
		public virtual ICollection<Comment> Comments { get; set; }
		public virtual ICollection<Vote> VotesNavigation { get; set; }
	}
}
