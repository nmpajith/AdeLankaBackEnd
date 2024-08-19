using AdeLankaBackEnd.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdeLankaBackEnd.Extentions
{
    public static class ModelBuilderExtentions
    {
        public static void CreateNoteUsersEntity(this ModelBuilder builder)
        {
            builder.Entity<NoteUser>().ToTable("NoteUsers");
            builder.Entity<NoteUser>().HasKey(p => p.Id);
            builder.Entity<NoteUser>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Entity<NoteUser>().Property(p => p.IdentityId).IsRequired();
            builder.Entity<NoteUser>().Property(p => p.DateCreated).IsRequired();
            builder.Entity<NoteUser>().Property(p => p.FirstName);
            builder.Entity<NoteUser>().Property(p => p.LastName);
            builder.Entity<NoteUser>().HasMany(p => p.Notes)
                .WithOne(p => p.NoteUser)
                .HasForeignKey(p => p.NoteUserId)
                .OnDelete(DeleteBehavior.NoAction) ;
            builder.Entity<NoteUser>()
                .HasMany(p => p.Comments)
                .WithOne(p => p.NoteUser)
                .HasForeignKey(p => p.NoteUserId)
                .OnDelete(DeleteBehavior.NoAction);
        }

        public static void CreateNotesEntity(this ModelBuilder builder)
        {
            builder.Entity<Note>().ToTable("Notes");
            builder.Entity<Note>().HasKey(p => p.Id);
            builder.Entity<Note>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Entity<Note>().Property(p => p.Title).IsRequired().HasMaxLength(255);
            builder.Entity<Note>().Property(p => p.DateCreated).IsRequired();
            builder.Entity<Note>().Property(p => p.Content).IsRequired();
            builder.Entity<Note>()
                .HasMany(p => p.Comments)
                .WithOne(p => p.Note)
                .HasForeignKey(p => p.NoteId)
                .OnDelete(DeleteBehavior.NoAction);
        }

        public static void CreateCommenstEntity(this ModelBuilder builder)
        {
            builder.Entity<Comment>().ToTable("Comments");
            builder.Entity<Comment>().HasKey(p => p.Id);
            builder.Entity<Comment>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Entity<Comment>().Property(p => p.Content).IsRequired();
            builder.Entity<Comment>().Property(p => p.DateCommented).IsRequired();
        }
    }
}
