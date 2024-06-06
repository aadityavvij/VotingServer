using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Xml.Linq;
using VotingServer.Models;

namespace VoterServer.Data
{
	public class ApplicationDbContext : IdentityDbContext<IdentityUser>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
		: base(options)
		{
		}

		public DbSet<Project> Projects { get; set; }
		public DbSet<Vote> Votes { get; set; }
		public DbSet<Comment> Comments { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Project>(entity =>
			{
				entity.HasKey(e => e.Id);

				entity.HasOne(e => e.CreatedBy)
					  .WithMany()
					  .HasForeignKey(e => e.CreatedById);

				entity.Property(e => e.CreatedAt);
			});

			modelBuilder.Entity<Vote>(entity =>
			{
				entity.HasKey(e => e.Id);

				entity.HasOne(e => e.User)
					  .WithMany()
					  .HasForeignKey(e => e.UserId);

				entity.HasOne(e => e.Project)
					  .WithMany(p => p.VotesNavigation)
					  .HasForeignKey(e => e.ProjectId);

				entity.Property(e => e.CreatedAt);
			});

			modelBuilder.Entity<Comment>(entity =>
			{
				entity.HasKey(e => e.Id);

				entity.HasOne(e => e.User)
					  .WithMany()
					  .HasForeignKey(e => e.UserId);

				entity.HasOne(e => e.Project)
					  .WithMany(p => p.Comments)
					  .HasForeignKey(e => e.ProjectId);

				entity.Property(e => e.CreatedAt);
			});
		}
	}
}
