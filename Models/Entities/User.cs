using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Project.Models.Entities
{
    [Table("User")]
    public class User
    {
        [Key]
        [Column("IDUser")]
        public int IDUser { get; set; }

        [MaxLength(20)]
        [Column("UserCode")]
        public string UserCode { get; set; }

        [Column("Phone")]
        [MaxLength(10)]
        public string Phone { get; set; }

        [Column("Password")]
        [MaxLength(50)]
        public string Password { get; set; }

        [Column("Email")]
        [MaxLength(80)]
        public string Email { get; set; }

        [Column("FullName")]
        [MaxLength(50)]
        public string FullName { get; set; }

        [Column("Dob")]
        public DateTime Dob { get; set; }

        [Column("Gender")]
        public bool Gender { get; set; }

        [Column("Role")]
        public byte Role { get; set; }

        [Column("TokenPassword")]
        [MaxLength(255)]
        public string? TokenPassword { get; set; }

        [Column("Points")]
        public int Points { get; set; }

        [Column("TotalSpending")]
        public int TotalSpending { get; set; }

        [Column("TokenLogin")]
        [MaxLength(255)]
        public string? TokenLogin { get; set; }
        public string? ZoneAddress { get; set; }
    }
}
