using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AsyncAPI.Entities
{
    [Table("Books")]
    public class Book
    {
        [Key] public int Id { get; set; }

        [Required] [MaxLength(150)] public string Title { get; set; }

        [MaxLength(2500)] public string Description { get; set; }

        public int AuthorId { get; set; }

        public Author Author { get; set; }
    }
}