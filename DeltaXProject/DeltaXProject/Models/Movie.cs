﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DeltaXProject.CustomValidator;

namespace DeltaXProject.Models
{
    public class Movie
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MovieID { get; set; }

        [Column("Name")]
        [Required]
        public string MovieName { get; set; }

        
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [CustomDateValidator]
        public DateTime YearOfRelease { get; set; }

        [Required,MaxLength(60, ErrorMessage = "Plot cannot be more than 60 characters")]
        public string Plot { get; set; }


        public byte[] Poster { get; set; }


        public virtual ICollection<Actor> Actors { get; set; }

        [ForeignKey("Producer")]
        public int ProducerID { get; set; }

        public virtual Producer Producer { get; set; }
    }
}