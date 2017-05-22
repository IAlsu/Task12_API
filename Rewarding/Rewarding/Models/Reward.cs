using Rewarding.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Rewarding.Models
{
    public class Reward: IEntity
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The field can't be empty")]
        [StringLength(50, ErrorMessage = "The length of the string must be less than 50 characters")]
        [RegularExpression("^[a-zA-Z0-9 -]+$", ErrorMessage = "Characters are not allowed.")]
        public string Title { get; set; }


        [StringLength(250, ErrorMessage = "The length of the string must be less than 250 characters")]
        public string Description { get; set; }

        public virtual ICollection<Person> Persons { get; set; }
        public Reward()
        {
            Persons = new List<Person>();
        }

        //[Required]
        public int? ImageId { get; set; }


        [ForeignKey("ImageId")]
        public Image Image { get; set; }
    }
}