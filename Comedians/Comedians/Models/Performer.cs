//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Comedians.Models
{
    using Metadata;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    //metadata partial class created so data annotations are retained
    //if the database is updated

    [MetadataType(typeof(PerformerMetadata))]
    public partial class Performer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Genre { get; set; }
        public string CountryOfOrigin { get; set; }
        public string YearsActive { get; set; }
        public string Biography { get; set; }
        public string PictureUrl { get; set; }
        public string VideoUrl { get; set; }
        public Nullable<int> Likes { get; set; }
        public Nullable<int> Dislikes { get; set; }
    }
}
