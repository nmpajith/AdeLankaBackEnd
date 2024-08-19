using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdeLankaBackEnd.DataTransferObjects
{
    public class CommentCreateDto
    {
        [Required]
        public string NoteTitle { get; set; }
        [Required]
        public DateTime NoteCreatedDate { get; set; }
        [Required]
        public string Content { get; set; }
    }
}
