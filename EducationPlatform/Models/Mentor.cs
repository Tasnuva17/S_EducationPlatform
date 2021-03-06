//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EducationPlatform.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Mentor
    {
        public Mentor()
        {
            this.Counselings = new HashSet<Counseling>();
            this.CourseDetails = new HashSet<CourseDetail>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public byte[] Photo { get; set; }
        public string Gender { get; set; }
        public string Institution { get; set; }
        public string IsValid { get; set; }
    
        public virtual ICollection<Counseling> Counselings { get; set; }
        public virtual ICollection<CourseDetail> CourseDetails { get; set; }
    }
}
