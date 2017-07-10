using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace HTMLGeneratorMVC.Models
{
    public class Style
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Please Enter Title!")]
        public string Title { get; set; }
        public string Category { get; set; }
        public string String1 { get; set; }
        public string String2 { get; set; }
        public string String3 { get; set; }
        public string String4 { get; set; }
        public string String5 { get; set; }
        public string String6 { get; set; }
        public DateTime Time { get; set; }

        [ForeignKey("User")]
        public int? UserId { get; set; }
        public virtual UserModel User { get; set; }
    }
}