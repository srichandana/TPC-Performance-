using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using TPC.Context.EntityModel;
using TPC.Core.Interfaces;


namespace TPC.Core
{
    public class EmailService : ServiceBase<IEmailModel>, IEmailService
    {
        public void SendMail(string subject, string body, bool isHtml, bool containsAttachment, List<string> files, MailMessage message = null, string frmEmail = null, string toEmail = null, string type = null, int quoteId = 0, string fromAddressDisplayName = null, int personID = 0, string quoteTitle = null)
        {
            if (message == null)
                message = new MailMessage();
            message.BodyEncoding = System.Text.Encoding.GetEncoding("utf-8");

            SmtpClient smtpClient = new SmtpClient(ConfigurationManager.AppSettings["Host"]);

            string msg = string.Empty;
            string frmAddress = string.Empty;
            string toAddress = string.Empty;
            string ccAddress = "penworthy1@gmail.com,ntcsofttech@gmail.com";
            if (Convert.ToInt32(ConfigurationManager.AppSettings["Testing"]) == 1)
            {
                ccAddress = ConfigurationManager.AppSettings["ccAddress"];
            }
            MailAddress fromAddress;


            if (string.IsNullOrEmpty(frmEmail))
                frmAddress = ConfigurationManager.AppSettings["fromAddress"];
            else
                frmAddress = frmEmail;

            if (Convert.ToInt32(ConfigurationManager.AppSettings["Testing"]) == 1)
            {
                toAddress = ConfigurationManager.AppSettings["WareHouseEmail"];
            }
            else
            {
                if (string.IsNullOrEmpty(toEmail))
                    toAddress = ConfigurationManager.AppSettings["WareHouseEmail"];
                else
                    toAddress = toEmail;
            }

            if (string.IsNullOrEmpty(fromAddressDisplayName))
            {
                fromAddress = new MailAddress(frmAddress, "WebSend");
            }
            else
            {
                fromAddress = new MailAddress(frmAddress, fromAddressDisplayName);
            }

            message.From = fromAddress;
            message.To.Add(toAddress);
            message.Bcc.Add(ccAddress);

            message.Subject = subject;
            message.IsBodyHtml = isHtml;
            message.Body = body;
            smtpClient.UseDefaultCredentials = true;
            if (containsAttachment)
            {
                if (files != null)
                {
                    foreach (string file in files)
                    {
                        message.Attachments.Add(new Attachment(file));
                    }
                }
            }

            smtpClient.Send(message);

            SaveMailHistory(frmAddress, toAddress, ccAddress, body, quoteId, type, personID, quoteTitle);
        }

        public void SendTestMail(string subject, string body, bool isHtml, bool containsAttachment, List<string> files, MailMessage message = null, string frmEmail = null, string toEmail = null, string type = null, int quoteId = 0, string fromAddressDisplayName = null, int personID = 0, string quoteTitle = null, MemoryStream byteAttach = null)
        {
            // message = new MailMessage();
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
            string msg = string.Empty;
            string frmAddress = string.Empty;
            string toAddress = string.Empty;

            message.From = new MailAddress("penworthytest@gmail.com");
            message.To.Add(ConfigurationManager.AppSettings["warehouseemail"]);
            message.Subject = subject;
            message.IsBodyHtml = isHtml;
            message.Body = body;

            smtpClient.Port = 587;
            smtpClient.Credentials = new System.Net.NetworkCredential("penworthytest@gmail.com", "password@321");
            if (byteAttach != null)
            {
                System.Net.Mime.ContentType ct = new System.Net.Mime.ContentType(System.Net.Mime.MediaTypeNames.Application.Pdf);
                System.Net.Mail.Attachment attach = new System.Net.Mail.Attachment(byteAttach, ct);
                attach.ContentDisposition.FileName = quoteId + ".pdf";
                message.Attachments.Add(attach);
            }

            smtpClient.EnableSsl = true;
            if (containsAttachment)
            {
                if (files != null)
                {
                    foreach (string file in files)
                    {
                        message.Attachments.Add(new Attachment(file));
                    }
                }
            }
            smtpClient.Send(message);
        }

        public bool SaveMailHistory(string frmAddress, string toAddress, string ccAddress, string body, int quoteId = 0, string type = null, int personID = 0, string quoteTitle = null)
        {
            MailHistory mailHistory = new MailHistory();
            mailHistory.FromEmail = frmAddress;
            mailHistory.To = toAddress;
            mailHistory.BCC = ccAddress;
            mailHistory.SendDate = DateTime.UtcNow;
            mailHistory.MailBody = body;
            mailHistory.type = type;
            mailHistory.quoteId = quoteId;
            mailHistory.PersonId = personID;
            mailHistory.QuoteTitle = quoteTitle;
            _Context.EmailHistory.Add(mailHistory);
            _Context.EmailHistory.SaveChanges();
            return true;
        }

        public void ServiceNotificationMail(string status, DateTime datetime)
        {
            if (Convert.ToInt32(ConfigurationManager.AppSettings["Testing"]) == 1)
            {
                FileStream fs = new FileStream(ConfigurationManager.AppSettings["CTFlatFile"] + "logs.txt", FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter m_streamWriter = new StreamWriter(fs);
                m_streamWriter.BaseStream.Seek(0, SeekOrigin.End);
                m_streamWriter.WriteLine("--------------");
                m_streamWriter.WriteLine(System.DateTime.Now.ToString());
                m_streamWriter.WriteLine(ConfigurationManager.AppSettings["CTFlatFile"] + "Mail sending is in process");
                m_streamWriter.WriteLine("--------------");
                m_streamWriter.Flush();
                m_streamWriter.Close();
            }
            string frmAddress = ConfigurationManager.AppSettings["fromAddress"];
            string toAddress = ConfigurationManager.AppSettings["NotificationEmail"];
            string ccAddress = "penworthy1@gmail.com,ntcsofttech@gmail.com";
            MailMessage message = new MailMessage();

            SmtpClient smtpClient = new SmtpClient(ConfigurationManager.AppSettings["Host"]);
            MailAddress fromAddress;

            fromAddress = new MailAddress(frmAddress, "WebSend");

            message.From = fromAddress;
            message.To.Add(toAddress);
            message.Bcc.Add(ccAddress);

            message.Subject = status;
            message.IsBodyHtml = false;
            message.Body = datetime.ToString();
            smtpClient.UseDefaultCredentials = true;

            smtpClient.Send(message);

        }

        private static void logexception(Exception ex)
        {
            string path = "E:\\logs.txt";
            if (!File.Exists(path))
            {
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine("--------------------------");
                    sw.WriteLine(DateTime.Now);
                    sw.WriteLine(ex.ToString());
                    sw.WriteLine("--------------------------");
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine("--------------------------");
                    sw.WriteLine(DateTime.Now);
                    sw.WriteLine(ex.ToString());
                    sw.WriteLine("--------------------------");
                }
            }
        }
    }
}
