using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Entities
{
    public class User
    {
        //[RegularExpression(@"^[A-Za-z]{3,}$", ErrorMessage = "Only letters are allowed")]
        public string FirstName { get; set; } = "";

        //[RegularExpression(@"^[A-Za-z]{3,}$", ErrorMessage = "Only letters are allowed")]
        public string LastName { get; set; } = "";

        //[RegularExpression(@"^[a-z]{5,100}@gmail.com$", ErrorMessage = "Invalid email format")]
        public string EmailId { get; set; } = "";

        //[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^@#$%*]).{8,}$", ErrorMessage = "Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one digit, and one special character")]
        public string Password { get; set; } = "";
    }
}
