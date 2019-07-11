using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeltaXProject.Models
{
    public class MovieViewModel
    {
        public Movie Movie { get; set; }
        public Actor Actor { get; set; }
        public Producer Producer{ get; set; }

}
}