using Rewarding.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rewarding.Models
{
    [Table("Persons")]
    public class Person: IEntity
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The field can't be empty")]
        [StringLength(50, ErrorMessage = "The length of the string must be less than 50 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The field can't be empty")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Birthdate { get; set; }

        [Range(0, 150, ErrorMessage = "Invalid age (must be between 0 and 150)")]
        public int Age { get; set; }

        public virtual ICollection<Reward> Rewards { get; set; }
        public Person()
        {
            Rewards = new List<Reward>();
        }

        public int? PhotoId { get; set; }
        [ForeignKey("PhotoId")]
        public Image Photo { get; set; }
    }
}

