using SendGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MailoWysylacz
{
    class Program
    {
        static void Main(string[] args)
        {

            SortedSet<String> set = new SortedSet<string>();




            string nstr = System.Console.ReadLine();

            int n = Convert.ToInt32(nstr);

            string row;

            for(int i = 0; i < n; i++)
            {

                row = System.Console.ReadLine();

                set.Add(row);


            }

            System.Text.StringBuilder str = new StringBuilder();


            nstr = System.Console.ReadLine();

            n = Convert.ToInt32(nstr);



            for (int i = 0; i < n; i++)
            {

                row = System.Console.ReadLine();

                str.Append(row);


            }



            //System.Console.Rea

            foreach (var email in set)
            {


                SendGridMessage myMessage = new SendGridMessage();
               // var email = listaMaili.list[i].emailAddress;
                myMessage.AddTo(email);
                myMessage.From = new MailAddress("", "");
                myMessage.Subject = "Spontime is here";
                myMessage.Text = "cokolwiek";//str.ToString();
                //myMessage.Html = str.ToString();
                var credentials = new NetworkCredential("", "");


                // Create an Web transport for sending email.
                var transportWeb = new Web(credentials);

                // Send the email, which returns an awaitable task.
                transportWeb.DeliverAsync(myMessage);
            }



        }



        }
    }

