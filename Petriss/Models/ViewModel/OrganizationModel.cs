using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace Petriss.Models.ViewModel
{
    public class OrganizationList
    {
        [Key]
        public int OrganizationId { get; set; }
      
        public string OrganizationName { get; set; }
              
    }

    

}


