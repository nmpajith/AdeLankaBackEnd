using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdeLankaBackEnd.DataTransferObjects
{
    public class CommentResponseDto
    {
        public string Content { get; set; }
        public DateTime DateCommented { get; set; }
        public string CommentCreatedBy { get; set; }
        public string NoteTitle { get; set; }
    }
}
