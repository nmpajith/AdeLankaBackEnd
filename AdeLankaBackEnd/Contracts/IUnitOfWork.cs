using AdeLankaBackEnd.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdeLankaBackEnd.Contracts
{
    public interface IUnitOfWork
    {
        IGenericRepository<NoteUser> NoteUsersRepository { get; }
        IGenericRepository<Comment> CommentsRepository { get; }
        IGenericRepository<Note> NotesRepository { get; }
        void Save();
        void Dispose();
    }
}
