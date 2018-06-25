using SpontimePictures.Models.LandingModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace SpontimePictures.Controllers
{
    public class LandingController : ApiController
    {


        [System.Web.Http.Route("Subscribe")]
        public async Task<IHttpActionResult> Subscribe(EmailAddress emailModel)
        {





            if (ModelState.IsValid)
            {
                using (SubscribeDatabaseEntities db = new SubscribeDatabaseEntities())
                {


                    if (await db.Subscribe.FirstOrDefaultAsync(e => e.Email == emailModel.email) == null)
                    {

                        Subscribe mail = new Subscribe();
                        mail.Email = emailModel.email;
                        mail.SendMail = false;


                        db.Subscribe.Add(mail);


                        try
                        {
                            await db.SaveChangesAsync();
                        }
                        catch
                        {
                            return BadRequest("already in database1");
                        }


                    }
                    else
                    {
                        return BadRequest("already in database2");
                    }


                }
                //emailModel.SendMail();

            }

            return Ok();
        }


        [System.Web.Http.Route("SendMailPost")]
        public async Task<IHttpActionResult> SendMailTo(EmailModel emailModel)
        {





            if (ModelState.IsValid)
            {
                await emailModel.SendGrid();
                //emailModel.SendMail();

            }

            return Ok();
        }

    }
}
