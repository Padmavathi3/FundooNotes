using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Entities
{
    public class UserNote
    {
        [Key]
        public string NoteId { get; set; } = "";
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public DateTime reminder { get; set; } 
        public string isArchive { get; set; } = "";
        public string isPinned { get; set; } = "";
        public string isTrash { get; set; }


        [ForeignKey("User")]
        public string EmailId { get; set; } = "";

    }
}
