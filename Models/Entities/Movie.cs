using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Project.Models.Entities
{
    [Table("Movies")]
    public class Movie
    {
        [Key]
        [Column("IDMovie")]
        public int IDMovie { get; set; }

        [Column("MovieName")]
        [MaxLength(255)]
        public string MovieName { get; set; }

        [Column("Genre")]
        [MaxLength(50)]
        public string Genre { get; set; }

        [Column("Duration")]
        public int Duration { get; set; }

        [Column("Description")]
        public string Description { get; set; }

        [Column("Director")]
        [MaxLength(50)]
        public string Director { get; set; }

        [Column("ReleaseDate")]
        public DateTime ReleaseDate { get; set; }

        [Column("CoverURL")]
        public string CoverURL { get; set; }

        [Column("TrailerURL")]
        public string TrailerURL { get; set; }
    }
}
