using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SpontimeCore.Models
{

   public class ShortEventModel
    {

        //List<string>UsersId
        public int Id { get; set; }
        public string OwnerId { get; set; }
        public string EventName { get; set; }
        public Nullable<System.DateTime> EventStartDate { get; set; }
        public int EventPriority { get; set; }
        public string EventLocalization { get; set; }

        public string PlaceId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }




        public ShortEventModel(Events dbEvent)
        {

            Id = dbEvent.Id;
            OwnerId = dbEvent.OwnerId;
            EventName = dbEvent.EventName;
            EventStartDate = dbEvent.EventStartDate;
            EventPriority = dbEvent.EventPriority;
            EventLocalization = dbEvent.EventLocalization;
            PlaceId = dbEvent.PlaceId;
            Latitude = dbEvent.Latitude ?? 181;
            Longitude = dbEvent.Longitude ?? 181;
            

        }

    }

    


   public class LongEventModel
    {

        public int Id { get; set; }
        public string OwnerId { get; set; }
        public string EventName { get; set; }
        public Nullable<System.DateTime> EventStartDate { get; set; }
        public Nullable<System.DateTime> EventEndDate { get; set; }
        public string EmojiId { get; set; }
        public string HashTag { get; set; }
        public int EventPriority { get; set; }


        public string PlaceId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string EventLocalization { get; set; }

        public List<string> AcceptInvitedUser;
        public List<string> NoActionInvitedUser;
        public List<string> CancelInvitedUser;


        public LongEventModel()
        {

        }

        public LongEventModel(Events dbEvent)
        {
            Id = dbEvent.Id;
            OwnerId = dbEvent.OwnerId;
            EventName = dbEvent.EventName;
            EventStartDate = dbEvent.EventStartDate;
            EventPriority = dbEvent.EventPriority;
            EventLocalization = dbEvent.EventLocalization;
            PlaceId = dbEvent.PlaceId;
            Latitude = dbEvent.Latitude ?? 181;
            Longitude = dbEvent.Longitude ?? 181;

            AcceptInvitedUser = new List<string>();
            NoActionInvitedUser = new List<string>();
            CancelInvitedUser = new List<string>();

            foreach (EventInvite inv in dbEvent.EventInvite)
            {
               if (inv.FriendId == OwnerId)
                { }
               else if (inv.EventInviteStatus == 1)
                {
                    AcceptInvitedUser.Add(inv.FriendId);
                }
                else if (inv.EventInviteStatus == 0)
                {
                    NoActionInvitedUser.Add(inv.FriendId);
                }
                else
                {
                    CancelInvitedUser.Add(inv.FriendId);
                }

            }
        }

  

        public LongEventModel Create(Events dbEvent)
        {
            Id = dbEvent.Id;
            OwnerId = dbEvent.OwnerId;
            EventName = dbEvent.EventName;
            EventStartDate = dbEvent.EventStartDate;
            EventPriority = dbEvent.EventPriority;
            EventLocalization = dbEvent.EventLocalization;
            PlaceId = dbEvent.PlaceId;
            Latitude = dbEvent.Latitude ?? 181;
            Longitude = dbEvent.Longitude ?? 181;

            AcceptInvitedUser = new List<string>();
            NoActionInvitedUser = new List<string>();
            CancelInvitedUser = new List<string>();

            foreach (EventInvite inv in dbEvent.EventInvite)
            {

                if (inv.FriendId == OwnerId)
                { }
                else if (inv.EventInviteStatus == 1)
                {
                    AcceptInvitedUser.Add(inv.FriendId);

                }
                else if (inv.EventInviteStatus == 0)
                {
                    NoActionInvitedUser.Add(inv.FriendId);
                }
                else
                {
                    CancelInvitedUser.Add(inv.FriendId);
                }

            }

            return this;
        }


    }


    public class EventsListModel
    {

        public List<ShortEventModel> notOwnedEventsList { get; set; }
        public List<ShortEventModel> ownedEventsList { get; set; }

        public async Task<EventsListModel> Create()
        {
            string CurrentUserId = HttpContext.Current.User.Identity.GetUserId();

            notOwnedEventsList = new List<ShortEventModel>();
            ownedEventsList = new List<ShortEventModel>();


            using (var db = new spontimeDatabaseEntities())
            {
               var user = await db.AspNetUsers.Include(u => u.EventInvite).Include("EventInvite.Events").FirstOrDefaultAsync(u => u.Id == CurrentUserId);

                List<EventInvite> invitesList = user.EventInvite.ToList();

                foreach(EventInvite inv in invitesList)
                {

                    if (inv.Events.OwnerId == CurrentUserId) ownedEventsList.Add(new ShortEventModel(inv.Events));
                    else
                    {
                        notOwnedEventsList.Add(new ShortEventModel(inv.Events));
                    }
                }
            }
            return this;
        }





    }
}