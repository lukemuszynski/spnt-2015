using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SpontimeCore.Models
{
    public class ShortUserModel
    {
        [Required]
        [StringLength(100)]

        public String UserId { get; set; }

        public String FacebookId { get; set; }

        public int Counter { get; set; }

        public string UserAvatarUrl { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }



        public ShortUserModel(AspNetUsers user)
        {

            UserId = user.Id;
            FacebookId = user.UserName;
            Counter = 0;
            UserAvatarUrl = user.AvatarUrl;
            FirstName = user.FirstName;
            LastName = user.LastName;


        }

        public ShortUserModel(string _UserId, string _UserName, string _FirstName, string _LastName, int _Counter, string _UserAvatarUrl)
        {
            UserId = _UserId;
            FacebookId = _UserName;
            Counter = _Counter;
            UserAvatarUrl = _UserAvatarUrl;
            FirstName = _FirstName;
            LastName = _LastName;

        }

    }
}