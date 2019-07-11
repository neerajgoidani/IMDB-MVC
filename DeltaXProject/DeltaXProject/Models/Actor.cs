using DeltaXProject.CustomValidator;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DeltaXProject.Models
{
    public class Actor
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Column("Name")]
        [Required]
        [MaxLength(15, ErrorMessage = "Length cannot be more than 15 characters")]
        public string ActorName { get; set; }

        [Required]
        public string Sex { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [CustomAgeValidator]
        public DateTime Dob { get; set; }

        [MaxLength(50, ErrorMessage = "Bio cannot be more than 50 characters")]
        public string Bio { get; set; }


        public virtual ICollection<Movie> Movies { get; set; }

    }
}