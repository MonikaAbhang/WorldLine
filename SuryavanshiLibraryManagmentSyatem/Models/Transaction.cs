using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SuryavanshiLibraryManagmentSyatem.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        [DisplayName("Book")]
        public string BookId { get; set; }
        [ForeignKey("BookId")]
        public Book Book { get; set; }
        public int CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateOfIssue { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateOfReturn { get; set; }

    }
}
