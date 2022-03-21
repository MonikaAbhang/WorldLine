using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SuryavanshiLibraryManagmentSyatem.Models
{
    public class Book
    {
        [Key]
        [Required(ErrorMessage = "ISBN cannot Be Edited")]
        public string ISBN { get; set; }
        public string Title { get; set; }
        public int PublisherId { get; set; }
        [ForeignKey("PublisherId")]
        public Publisher Publisher { get; set; }
        public int AuthorId { get; set; }
        [ForeignKey("AuthorId")]
        public Author Author { get; set; }
        public bool IssuedStatus { get; set; }
        public bool IsDeleted { get; set; }

    }
}
