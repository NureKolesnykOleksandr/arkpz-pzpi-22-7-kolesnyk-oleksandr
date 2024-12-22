﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ServerMM.Models
{
    public class UserLogin
    {
        [Key]
        public int LoginID { get; set; }

        [Required]
        public int UserID { get; set; }

        [ForeignKey("UserID")]
        public User User { get; set; }

        public DateTime LoginTime { get; set; } = DateTime.Now;

        [MaxLength(45)]
        public string IPAddress { get; set; }
    }
}
