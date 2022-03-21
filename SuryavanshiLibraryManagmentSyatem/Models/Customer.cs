using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SuryavanshiLibraryManagmentSyatem.Models
{
    public class Customer
    {
        [Key]
        [Required(ErrorMessage = "Id cannot Be Edited")]
        public int Id { get; set; }
        public string Name { get; set; }
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not Valid Please Enter valid Email address")]
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public bool IsDeleted { get; set; }
    }
}
