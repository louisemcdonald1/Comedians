using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Comedians.Models.Metadata
{
    //metadata partial class created so data annotations are retained
    //if the database is updated
    public partial class PerformerMetadata
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Genre { get; set; }
        [Display(Name = "Country of Origin")]
        public string CountryOfOrigin { get; set; }
        [Display(Name = "Years Active")]
        public string YearsActive { get; set; }
        public string Biography { get; set; }
        [Display(Name = "Picture (URL)")]
        public string PictureUrl { get; set; }
        [Display(Name = "Video (URL)")]
        public string VideoUrl { get; set; }
        public Nullable<int> Likes { get; set; }
        public Nullable<int> Dislikes { get; set; }

    }//end class
}//end namespace