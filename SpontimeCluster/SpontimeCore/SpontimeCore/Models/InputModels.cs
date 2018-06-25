using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SpontimeCore.Models
{
    public class InputModels
    {

       public class EventIdInput
        {
            [Required]
            [Range(0,int.MaxValue)]
            public int EventId { get; set; }
              

        }


        public class UserIdListInput
        {
            [Required]
            [MaxLength(50)]
            public List<string> UserIdList { get; set; }

        }


    }
}