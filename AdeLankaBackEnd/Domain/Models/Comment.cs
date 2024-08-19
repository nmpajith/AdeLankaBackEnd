using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdeLankaBackEnd.Domain.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime DateCommented { get; set; }
        public int NoteUserId { get; set; }
        public NoteUser NoteUser { get; set; }
        public int NoteId { get; set; }
        public Note Note { get; set; }
    }
}
