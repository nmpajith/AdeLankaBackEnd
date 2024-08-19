using AdeLankaBackEnd.Domain.Models;
using AdeLankaBackEnd.Extentions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdeLankaBackEnd.Persistence.Contexts
{
    public class AppDbContext : IdentityDbContext 
    {
        public AppDbContext(DbContextOptions options)
            : base(options)
        {
        }
        public DbSet<NoteUser> NoteUsers { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.CreateNoteUsersEntity();
            builder.CreateNotesEntity();
            builder.CreateCommenstEntity();
        }
    }
}