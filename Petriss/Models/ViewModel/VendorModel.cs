using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace Petriss.Models.ViewModel
{
    public class VendorList
    {
        [Key]
        public int VendorId { get; set; }
      
        public string VendorName { get; set; }
              
    }

    

}


