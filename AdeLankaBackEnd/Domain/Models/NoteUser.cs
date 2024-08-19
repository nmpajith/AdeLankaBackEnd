using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdeLankaBackEnd.Domain.Models
{
    public class NoteUser
    {
        public int Id { get; set; }
        public string IdentityId { get; set; }
        public DateTime DateCreated { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<Note> Notes { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
