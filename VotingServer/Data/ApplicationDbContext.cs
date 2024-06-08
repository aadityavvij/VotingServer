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

			// AspNetUserTokens
			modelBuilder.Entity<IdentityUserToken<string>>(entity =>
			{
				entity.Property(e => e.UserId).HasMaxLength(127);
				entity.Property(e => e.LoginProvider).HasMaxLength(128);
				entity.Property(e => e.Name).HasMaxLength(128);
			});

			// AspNetUserLogins
			modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
			{
				entity.Property(e => e.UserId).HasMaxLength(127);
				entity.Property(e => e.LoginProvider).HasMaxLength(128);
				entity.Property(e => e.ProviderKey).HasMaxLength(128);
			});

			// AspNetUsers
			modelBuilder.Entity<IdentityUser>(entity =>
			{
				entity.Property(e => e.Id).HasMaxLength(127);
				entity.Property(e => e.Email).HasMaxLength(127);
				entity.Property(e => e.NormalizedEmail).HasMaxLength(127);
				entity.Property(e => e.NormalizedUserName).HasMaxLength(127);
				entity.Property(e => e.LockoutEnd).HasColumnType("datetime");
			});

			// AspNetRoles
			modelBuilder.Entity<IdentityRole>(entity =>
			{
				entity.Property(e => e.Id).HasMaxLength(127);
				entity.Property(e => e.Name).HasMaxLength(128);
				entity.Property(e => e.NormalizedName).HasMaxLength(127);
			});

			// AspNetRoleClaims
			modelBuilder.Entity<IdentityRoleClaim<string>>(entity =>
			{
				entity.Property(e => e.RoleId).HasMaxLength(127);
			});

			// AspNetUserClaims
			modelBuilder.Entity<IdentityUserClaim<string>>(entity =>
			{
				entity.Property(e => e.UserId).HasMaxLength(127);
			});

			// AspNetUserRoles
			modelBuilder.Entity<IdentityUserRole<string>>(entity =>
			{
				entity.Property(e => e.UserId).HasMaxLength(127);
				entity.Property(e => e.RoleId).HasMaxLength(127);
			});

			// Projects
			modelBuilder.Entity<Project>(entity =>
			{
				entity.HasKey(e => e.Id);

				entity.HasOne(e => e.CreatedBy)
					  .WithMany()
					  .HasForeignKey(e => e.CreatedById);

				entity.Property(e => e.CreatedAt).HasColumnType("datetime");
			});

			// Votes
			modelBuilder.Entity<Vote>(entity =>
			{
				entity.HasKey(e => e.Id);

				entity.HasOne(e => e.User)
					  .WithMany()
					  .HasForeignKey(e => e.UserId);

				entity.HasOne(e => e.Project)
					  .WithMany(p => p.VotesNavigation)
					  .HasForeignKey(e => e.ProjectId);

				entity.Property(e => e.CreatedAt).HasColumnType("datetime");
			});

			// Comments
			modelBuilder.Entity<Comment>(entity =>
			{
				entity.HasKey(e => e.Id);

				entity.HasOne(e => e.User)
					  .WithMany()
					  .HasForeignKey(e => e.UserId);

				entity.HasOne(e => e.Project)
					  .WithMany(p => p.Comments)
					  .HasForeignKey(e => e.ProjectId);

				entity.Property(e => e.CreatedAt).HasColumnType("datetime");
			});
		}
	}
}
