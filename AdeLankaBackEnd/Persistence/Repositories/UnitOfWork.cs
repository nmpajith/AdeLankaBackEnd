using AdeLankaBackEnd.Contracts;
using AdeLankaBackEnd.Domain.Models;
using AdeLankaBackEnd.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdeLankaBackEnd.Persistence.Repositories
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private AppDbContext _context;
        private IGenericRepository<NoteUser> _noteUsersRepository;
        private IGenericRepository<Note> _notesRepository;
        private IGenericRepository<Comment> _comentssRepository;
        ILoggerManager _logger;

        public UnitOfWork(AppDbContext context, ILoggerManager logger)
        {
            _context = context;
            _logger = logger;
        }

        public IGenericRepository<NoteUser> NoteUsersRepository
        {
            get
            {

                if (_noteUsersRepository == null)
                {
                    _noteUsersRepository = new GenericRepository<NoteUser>(_context, _logger);
                }
                return _noteUsersRepository;
            }
        }

        public IGenericRepository<Comment> CommentsRepository
        {
            get
            {

                if (_comentssRepository == null)
                {
                    _comentssRepository = new GenericRepository<Comment>(_context, _logger);
                }
                return _comentssRepository;
            }
        }

        public IGenericRepository<Note> NotesRepository
        {
            get
            {

                if (_notesRepository == null)
                {
                    _notesRepository = new GenericRepository<Note>(_context, _logger);
                }
                return _notesRepository;
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
