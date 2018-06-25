using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SpontimeCore.Models
{
    public class MyEventsModel
    {

        public List<EventModel> myEventsList;


        public async Task<MyEventsModel> CreateAsync()
        {
            myEventsList = new List<EventModel>();
            String CurrentUserId = HttpContext.Current.User.Identity.GetUserId();

            using (var db = new spontimeDatabaseEntities())
            {

                var currUserQuery = (from usr in db.AspNetUsers.Include(e => e.EventInvite) where usr.Id == CurrentUserId select usr);
                currUserQuery.Include("EventInvites.Events");
               // currUserQuery.Include("EventInvites.Events.EventInvites");
                var currUser = await currUserQuery.FirstOrDefaultAsync();
               
                foreach(var inv in currUser.EventInvite)
                {


                    myEventsList.Add(inv.Events);

                }



            }

        }

    }
}