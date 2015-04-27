using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using TPC.Context.EntityModel;
using TPC.Core.Interfaces;
using TPC.Core.Models.Models;
using TPC.Core.Models.ViewModels;

namespace TPC.Core
{
    public class EmailBodyGeneratorService : ServiceBase<IEmailTemplateModel>
    {
        public void InitialiseHTMLParser(EmailTemplateViewModel item, string template, string subject, string type = null)
        {
            EmailService mailService = new EmailService();
            Hashtable templateVars = new Hashtable();
            templateVars.Add("DefaultDW", item.EmailDWTemplateList.FirstOrDefault().DWName);

            HTMLParserService parser;
            string baseTemplatepath = ConfigurationManager.AppSettings["EmailTemplate"];

            switch (template.ToLower())
            {
                case "initial":
                    parser = new HTMLParserService(baseTemplatepath + "TemplateInitial.html", templateVars);
                    break;
                case "idle":
                    parser = new HTMLParserService(baseTemplatepath + "TemplateIdle.html", templateVars);
                    break;
                case "active":
                    parser = new HTMLParserService(baseTemplatepath + "TemplateActive.html", templateVars);
                    break;
                case "outstanding":
                    parser = new HTMLParserService(baseTemplatepath + "TemplateOutstanding.html", templateVars);
                    break;
                case "initialtext":
                    parser = new HTMLParserService(baseTemplatepath + "TemplateInitialText.html", templateVars);
                    break;
                case "initialnewuser":
                    parser = new HTMLParserService(baseTemplatepath + "TemplateInitalNewUser.html", templateVars);
                    break;
                default:
                    parser = new HTMLParserService(baseTemplatepath + "TemplateIdle.html", templateVars);
                    break;
            }


            string DefaultDWLink = GenerateAnchorList(item.EmailDWTemplateList, parser, "DefaultDWLink");
            if (!string.IsNullOrEmpty(DefaultDWLink))
                templateVars.Add("DefaultDWLink", DefaultDWLink);

            //added by chandana for rep Comments
            bool ContainsRepComments = !string.IsNullOrEmpty(item.RepComments) ? true : false;
            templateVars.Add("ContainsRepComments", ContainsRepComments);
            string RepComments = GenerateRepCommentsText(item, parser);
            templateVars.Add("RepComments", RepComments);

            string videoLink = GenerateAnchorList(item.EmailDWTemplateList, parser, "VideoLink");
            if (!string.IsNullOrEmpty(videoLink))
                templateVars.Add("VideoLink", videoLink);

            string RepDetails = GenerateAnchorList(item.EmailDWTemplateList, parser, "RepDetails", item);
            templateVars.Add("RepDetails", RepDetails);


            bool isMultipleRows = item.EmailDWTemplateList.Count > 1 ? true : false;
            templateVars.Add("IsMultipleRows", isMultipleRows);

            bool ContainsItems = item.EmailDWTemplateList.FirstOrDefault().LstISBN.Count == 0 ? false : true;
            templateVars.Add("ContainsItems", ContainsItems);


            //if (template.ToLower() != "initial")
            //{

            string anchorList = GenerateAnchorList(item.EmailDWTemplateList, parser, "Link");
            if (!string.IsNullOrEmpty(anchorList))
                templateVars.Add("Link", anchorList);
            //}

            MailMessage message = new MailMessage();

            string mailBody = string.Empty;
            //added by faraaz
            if (template.ToLower() != "initialnewuser" && template.ToLower() != "initialtext")
            {
                EmailLinkedResourceModel imageLinkedResource = GenerateImageList(item.EmailDWTemplateList, parser);
                if (!string.IsNullOrEmpty(imageLinkedResource.HtmlImageContent))
                    templateVars.Add("image", imageLinkedResource.HtmlImageContent);
                mailBody = parser.Parse();
                //Check if image resource is attached
                AlternateView view = AlternateView.CreateAlternateViewFromString(mailBody, null, System.Net.Mime.MediaTypeNames.Text.Html);
                if (imageLinkedResource.ListImageResource.Count > 0)
                {
                    foreach (LinkedResource lstimage in imageLinkedResource.ListImageResource)
                    {
                        view.LinkedResources.Add(lstimage);
                    }
                    message.AlternateViews.Add(view);
                }
            }


            if (item.EmailDWTemplateList.FirstOrDefault() != null)
            {
                string emailType = string.IsNullOrEmpty(type) && type == null ? "DW" : type;
                if (!IsEmailSent(item.EmailDWTemplateList.FirstOrDefault().QuoteID, item.PersonID, item.FromAddress, item.ToAddress, DateTime.UtcNow, emailType))
                {
                    mailBody = parser.Parse();
                    //Enabled dynamic "from email" i.e item.FromAddress instead of "null" to test sending email from associated rep to Users.
                    if (!string.IsNullOrEmpty(item.ToAddress) && !string.IsNullOrEmpty(item.FromAddress))
                        mailService.SendMail(subject, mailBody, true, false, null, message, item.FromAddress, item.ToAddress, emailType, item.EmailDWTemplateList.FirstOrDefault().QuoteID, item.DisplayName, item.PersonID, item.EmailDWTemplateList.FirstOrDefault().DWName);
                }
            }

        }



        private bool IsEmailSent(int quoteID, int personID, string fromEmail, string toEmail, DateTime datetime, string emailType)
        {
            List<MailHistory> lstmailhistory = _Context.EmailHistory.GetAll(e => e.quoteId == quoteID && e.PersonId == personID && e.type == emailType).Where(e => e.SendDate.ToShortDateString() == datetime.ToShortDateString()).ToList();
            if (lstmailhistory != null && lstmailhistory.Count() > 0)
            {
                return true;
            }

            return false;
        }

        private string ImageToBase64String(string filepath)
        {
            byte[] imageArray = System.IO.File.ReadAllBytes(filepath);
            return Convert.ToBase64String(imageArray);
        }

        private EmailLinkedResourceModel GenerateImageList(List<EmailTemplateModel> lstEmailTemplatemodel, HTMLParserService parser)
        {
            Hashtable imgpaths = new Hashtable();
            StringBuilder imagesblock = new StringBuilder();
            List<LinkedResource> lstImageResouces = new List<LinkedResource>();
            LinkedResource resource;
            MailMessage message = new MailMessage();
            EmailLinkedResourceModel imagelinkResource = new EmailLinkedResourceModel();

            List<string> lstIsbns = lstEmailTemplatemodel.FirstOrDefault() != null ? lstEmailTemplatemodel.FirstOrDefault().LstISBN : new List<string>();
            foreach (string isbn in lstIsbns)
            {
                string imagePath = ConfigurationManager.AppSettings["CommonRepository"] + ConfigurationManager.AppSettings["ImagesH135"] + @"\";
                if (imgpaths.Count < 5)
                {
                    if (!isbn.Contains(imagePath))
                        imagePath = imagePath + isbn + ".jpg";
                    else
                        imagePath = isbn;
                    if (System.IO.File.Exists(imagePath))
                    {
                        imgpaths.Add(lstEmailTemplatemodel.FirstOrDefault().DWName + "-" + isbn, imagePath);
                    }
                }
            }


            IDictionaryEnumerator myEnum = imgpaths.GetEnumerator();
            int i = 1;
            while (myEnum.MoveNext())
            {
                string imgFile = myEnum.Value.ToString();
                resource = new LinkedResource(imgFile);
                resource.ContentId = "id" + i;
                lstImageResouces.Add(resource);

                Hashtable blockVars = new Hashtable();
                blockVars.Add("imgpath", "id" + i);
                blockVars.Add("DWName", myEnum.Key);

                string parsedHtml = parser.ParseBlock("image", blockVars);

                if (!string.IsNullOrEmpty(parsedHtml))
                    imagesblock.Append(parsedHtml);

                i = i + 1;
            }

            imagelinkResource.HtmlImageContent = imagesblock.ToString();
            imagelinkResource.ListImageResource = lstImageResouces;
            return imagelinkResource;
        }

        private string GenerateAnchorList(List<EmailTemplateModel> lstEmailTemplatemodel, HTMLParserService parser, string type, EmailTemplateViewModel item = null)
        {
            Hashtable links = new Hashtable();
            string dwBaseUrl = ConfigurationManager.AppSettings["DWEmailUrl"];
            string dwVideoUrl = ConfigurationManager.AppSettings["DWVideoUrl"];

            if (type == "DefaultDWLink")
            {
                EmailTemplateModel etm = lstEmailTemplatemodel.Take(1).FirstOrDefault();
                links.Add(etm.DWName + "~" + etm.QuoteID, dwBaseUrl + etm.QuoteID + "&type=Decision Wizard");
            }
            else if (type == "Link")
            {
                foreach (EmailTemplateModel etm in lstEmailTemplatemodel.Skip(1))
                {
                    links.Add(etm.DWName + "~" + etm.QuoteID, dwBaseUrl + etm.QuoteID + "&type=Decision Wizard");
                }
            }
            else if (type == "VideoLink")
            {
                links.Add("videolink", dwVideoUrl);
            }
            else
            {
                if (item != null)
                {
                    links.Add("repDetails", item.RepFirstName + "~" + item.FromAddress);
                }
            }
            //generate others DW's list  
            StringBuilder linksBlock = new StringBuilder();
            IDictionaryEnumerator myEnumerator = links.GetEnumerator();
            while (myEnumerator.MoveNext())
            {
                Hashtable blockVars = new Hashtable();
                if (type != "VideoLink")
                {
                    if (item != null)
                    {
                        blockVars.Add("RepName", myEnumerator.Value.ToString().Split('~')[0]);
                        blockVars.Add("RepPhone", "");
                        blockVars.Add("RepEmailAddress", myEnumerator.Value.ToString().Split('~')[1]);
                    }
                    else
                    {
                        blockVars.Add("Url", myEnumerator.Value);
                        blockVars.Add("DWName", myEnumerator.Key.ToString().Split('~')[0]);
                    }
                }
                else
                {
                    blockVars.Add("VideoUrl", myEnumerator.Value);

                }
                string parsedHtml = parser.ParseBlock(type, blockVars);

                if (!string.IsNullOrEmpty(parsedHtml))
                    linksBlock.Append(parsedHtml);
            }

            return linksBlock.ToString();
        }


        private string GenerateRepCommentsText(EmailTemplateViewModel item, HTMLParserService parser)
        {
            Hashtable blockVars = new Hashtable();
            blockVars.Add("RepComments", item.RepComments);
            string parsedHtml = parser.ParseBlock("RepComments", blockVars);
            return parsedHtml;
        }

    }
}
