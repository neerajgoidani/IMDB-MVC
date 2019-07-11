using DeltaXProject.CustomValidator;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeltaXProject.Models
{
    public class Producer
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProducerID { get; set; }

        [Column("Name")]
        [Required]
        public string ProducerName { get; set; }

        [Required]
        public string Sex { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [CustomAgeValidator]
        public DateTime Dob { get; set; }

        [Required,MaxLength(15, ErrorMessage = "Length cannot be more than 20 characters")]
        public string Bio { get; set; }

        public virtual ICollection<Movie> Movies { get; set; }

    }
}