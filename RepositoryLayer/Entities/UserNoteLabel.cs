using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Entities
{
    public class UserNoteLabel
    {
        public string LabelName { get; set; } = "";
        
        [ForeignKey("UserNote")]
        public string NoteId { get; set; } = "";

        [ForeignKey("User")]
        public string Email { get; set; } = "";
    }
}
