using Microsoft.AspNet.Identity;
using SpontimeCore.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace SpontimeCore.Controllers
{
    [Authorize]
    public class EventController : ApiController
    {


        [Route("GetLongEventInfo")]
        public async Task<LongEventModel> GetEventById([FromBody]InputModels.EventIdInput eventId)
        {
            string CurrUserId = HttpContext.Current.User.Identity.GetUserId();
            LongEventModel ret;

            using (var db = new spontimeDatabaseEntities())
            {
                var ev = await (from e in db.Events.Include(iii => iii.EventInvite) where e.Id == eventId.EventId select e).FirstOrDefaultAsync();
                ret = new LongEventModel(ev);
            }
            return ret;
        }



        [Route("GetEventsList")]
        public async Task<EventsListModel> MyEventsList()
        {

            EventsListModel ret = new EventsListModel();
            await ret.Create();

            return ret;

        }


    }
}