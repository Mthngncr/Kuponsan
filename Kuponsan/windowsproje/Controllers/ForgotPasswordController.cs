using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using windowsproje.Models;
using System.Web.Security;
using System.Net.Mail;
using System.Net;

namespace windowsproje.Controllers
{
    public class ForgotPasswordController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Index(Kullanıcılar u)
        {

          
                if (ModelState.IsValid)
                {

                using (kupansandbEntities1 dc = new kupansandbEntities1())
                {
                    var v = dc.Kullanıcılar.Where(a => a.Username.Equals(u.Username) && a.Yetki.Equals(1)).FirstOrDefault();
                    if (v != null)
                    {
                        MailMessage mailMessage = new MailMessage();
                        mailMessage.Subject = "Şifre Hatırlatma Hakkında";
                        mailMessage.Body = "\n Merhabalar " + v.FullName +
                            "\n \n Sisteme kayıtlı şifreniz: " + v.Password +
                            "\n \n İyi günler dileriz.";
                        mailMessage.From = new MailAddress("kuponsan@gmail.com", "KuponsanCompany");
                        mailMessage.CC.Add(new MailAddress("kuponsan@gmail.com", "KuponsanCompany"));
                        mailMessage.To.Add(new MailAddress(v.Mail, "Receiver_" + v.Username));

                        SmtpClient mySmtpClient = new SmtpClient();
                        mySmtpClient.Host = "smtp.gmail.com";
                        mySmtpClient.EnableSsl = true;
                        mySmtpClient.Port = 587;
                        mySmtpClient.Send(mailMessage);

                        ModelState.AddModelError("", "Mail gönderildi.");

                      
                       
                    }

                    else
                    {
                        ModelState.AddModelError("", "Kullanıcı adı veya şifre yanlış");
                    }
                }
            }
            return View(u);



        }


    }
}