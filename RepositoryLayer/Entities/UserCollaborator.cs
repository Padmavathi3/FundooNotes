using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Entities
{
    public class UserCollaborator
    {
        [Key]
        public string CollaboratorId { get; set; } = "";

        [ForeignKey("UserNote")]
        public string NoteId { get; set; } = "";

        [ForeignKey("User")]
        public string CollaboratorEmail { get; set; } = "";

    }
}
