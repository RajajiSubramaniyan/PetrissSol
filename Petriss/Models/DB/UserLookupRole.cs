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
    
    public partial class UserLookupRole
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UserLookupRole()
        {
            this.UsersRoles = new HashSet<UsersRole>();
        }
    
        public int UserLookupRoleId { get; set; }
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }
        public int CreatedByUserId { get; set; }
        public Nullable<System.DateTime> CreatedDateTime { get; set; }
        public int ModifiedByUserId { get; set; }
        public Nullable<System.DateTime> ModifiedDateTime { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UsersRole> UsersRoles { get; set; }
    }
}