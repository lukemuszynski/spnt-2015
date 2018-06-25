using SendGrid;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace SpontimePictures.Models.LandingModels
{
    public class EmailAddress
    {

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string email { get; set; }


    }


    public class EmailModel
    {

        [Required]
        [StringLength(60)]
        public string imie { get; set; }

        // string email,string tresc
        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string email { get; set; }

        [Required]
        [StringLength(6000)]
        public string tresc { get; set; }


        public EmailModel(string _imie, string _email, string _tresc)
        {

            imie = _imie;
            email = _email;
            tresc = _tresc;

        }


        public async Task SendGrid()
        {

            var myMessage = new SendGridMessage();

            // Add the message properties.
            myMessage.From = new MailAddress(email);

            // Add multiple addresses to the To field.
            List<String> recipients = new List<String>
                {
                    // @"Karolina Demianczuk <demianczukkarolina@gmail.com>",
                     @"Łukasz Muszyński <nathorik@gmail.com>",
                     @"spontime info <info@spontime.co>"
                };

            myMessage.AddTo(recipients);

            myMessage.Subject = "Landing Mail from: " + imie;

            //Add the HTML and Text bodies

            myMessage.Text = tresc;

            var credentials = new NetworkCredential("azure_7d0579c06b4dc51abb8ad1a330e2bee3@azure.com", "SpontimeHaslo12345!");

            // Create an Web transport for sending email.
            var transportWeb = new Web(credentials);

            // Send the email, which returns an awaitable task.


            try {
               await transportWeb.DeliverAsync(myMessage);
            }
            catch
            {

                int x = 1;

            }
            // If developing a Console Application, use the following
            // transportWeb.DeliverAsync(mail).Wait();



        }




    }
}