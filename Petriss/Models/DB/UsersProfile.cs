//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Petriss.Models.DB
{
    using System;
    using System.Collections.Generic;
    
    public partial class UsersProfile
    {
        public int UserProfileId { get; set; }
        public int UserId { get; set; }
        public string PreferredName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public Nullable<int> StatusId { get; set; }
        public string Gender { get; set; }
        public Nullable<int> StateId { get; set; }
        public Nullable<int> CityId { get; set; }
        public Nullable<int> OrganizationId { get; set; }
        public int CreatedByUserId { get; set; }
        public Nullable<System.DateTime> CreatedDateTime { get; set; }
        public int ModifiedByUserId { get; set; }
        public Nullable<System.DateTime> ModifiedDateTime { get; set; }
    
        public virtual User User { get; set; }
    }
}
