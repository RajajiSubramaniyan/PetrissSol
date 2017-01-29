using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace Petriss.Models.ViewModel
{
    public class InstrumentList
    {
        [Key]
        public int InstrumentId { get; set; }
      
        public string InstrumentName { get; set; }
              
    }

    

}


