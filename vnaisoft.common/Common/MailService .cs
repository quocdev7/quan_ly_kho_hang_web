

using Microsoft.Extensions.Options;
using quan_ly_kho.common.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace quan_ly_kho.common.Common
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;
        public MailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }
        public  void Email(string htmlString, string Subject, string MailTo, string MailCc)
        {
            try
            {
                string[] arr_MailTo;
                string[] arr_MailCc;

                MailTo = MailTo ?? "";
                 MailCc = MailCc ?? "";
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress(_mailSettings.Mail,_mailSettings.DisplayName);
                if (!string.IsNullOrEmpty(MailTo))
                {
                    arr_MailTo = MailTo.Split(';');
                    foreach (string value in arr_MailTo)
                    {
                        message.To.Add(new MailAddress(value));
                    }
                }
                if (!string.IsNullOrEmpty(MailCc))
                {
                    arr_MailCc = MailCc.Split(';');
                    foreach (string value in arr_MailCc)
                    {
                        message.CC.Add(new MailAddress(value));
                    }
                }

              

                //message.CC.Add(new MailAddress("hoang.vu@vnaisoft.com.vn"));
                message.Subject = Subject;
                message.IsBodyHtml = true; //to make message body as html  
                message.Body = htmlString;
                smtp.Port = 587;
                smtp.Host =_mailSettings.Host; //for gmail host  
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(_mailSettings.Mail, _mailSettings.Password);
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.SendMailAsync(message);
            }
            catch (Exception ex)
            {
              // Functions.ShowMsg(ex.ToString());
            }
        }


        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            //var email = new MimeMessage();
            //email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            //email.Subject = mailRequest.Subject;
            //var builder = new BodyBuilder();
            //if (mailRequest.Attachments != null)
            //{
            //    byte[] fileBytes;
            //    foreach (var file in mailRequest.Attachments)
            //    {
            //        if (file.Length > 0)
            //        {
            //            using (var ms = new MemoryStream())
            //            {
            //                file.CopyTo(ms);
            //                fileBytes = ms.ToArray();
            //            }
            //            builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
            //        }
            //    }
            //}
            //builder.HtmlBody = mailRequest.Body;
            //email.Body = builder.ToMessageBody();
            //using var smtp = new SmtpClient();
           
          
            try
            {
                Email(mailRequest.Body, mailRequest.Subject, mailRequest.ToEmail, mailRequest.CCEmail);

            }
            catch (Exception e)
            {
              
            }
           
        }
    }
}
