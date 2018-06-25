using SpontimeCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace SpontimeCore.Controllers
{
    [Authorize]
    public class UserController : ApiController
    {

        public async Task<ReturnModels.ShortUserModelList> GetShortUserModels(InputModels.UserIdListInput usersList)
        {

            List<ShortUserModel> shortUserModelsList = new List<ShortUserModel>();

            using(var db = new spontimeDatabaseEntities())
            {
                foreach(string userId in usersList.UserIdList)
                {
                    AspNetUsers user = await db.AspNetUsers.FindAsync(userId);
                    if (user != null)
                    {
                        shortUserModelsList.Add(new ShortUserModel(user));
                    }
                }
               }


            ReturnModels.ShortUserModelList ret = new ReturnModels.ShortUserModelList(shortUserModelsList);


            return ret;
        }


    }
}