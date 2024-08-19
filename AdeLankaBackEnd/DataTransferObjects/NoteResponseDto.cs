using AdeLankaBackEnd.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdeLankaBackEnd.DataTransferObjects
{
    public class NoteResponseDto
    {
        public string CreatedBy { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
        public ICollection<CommentResponseDto> Comments { get; set; }
    }
}
