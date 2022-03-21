using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SuryavanshiLibraryManagmentSyatem.Models
{
    public class Author
    {   
            [Key]
            public int Id { get; set; }
            public string Name { get; set; }
            public string Notes { get; set; }
        
    }
}

