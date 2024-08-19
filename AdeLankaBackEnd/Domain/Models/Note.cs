using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdeLankaBackEnd.Domain.Models
{
    public class Note
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
        public int NoteUserId { get; set; }
        public NoteUser NoteUser { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
