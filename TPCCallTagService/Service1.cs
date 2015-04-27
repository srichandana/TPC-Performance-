using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TPC.Core;
using TPC.Core.Models.Models;
using TPC.Core.Models.ViewModels;


namespace TPCCallTagService
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }


        string conn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        protected override void OnStart(string[] args)
        {
            try
            {

                EmailService mailService = new EmailService();
                mailService.ServiceNotificationMail("DW Remainder Email Service Started", DateTime.Now);
                // Thread.Sleep(10000);//simulate 5 minutes work

                this.RequestAdditionalTime(600000);// avoid service time out for 10 mins

                //Scheduler timer for CallTag execution
                // SetCallTagTimer();

                //process DW's Mail Reminder
                SetDWEmailReminderTimer();

            }
            catch (Exception ex)
            {
                Elmah.SqlErrorLog ErrorLog = new Elmah.SqlErrorLog(conn);
                ErrorLog.Log(new Elmah.Error(new Exception(ex.ToString())));
            }
        }

        protected override void OnStop()
        {
            try
            {
                EmailService mailService = new EmailService();
                mailService.ServiceNotificationMail("DW Remainder Email Service Stopped", DateTime.Now);
                Console.WriteLine("Stopped");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Elmah.SqlErrorLog ErrorLog = new Elmah.SqlErrorLog(conn);
                ErrorLog.Log(new Elmah.Error(new Exception(ex.ToString())));
            }
        }

        #region CallTag - Flat File creation

        private void GenerateFlatFiles()
        {
            try
            {
                EmailService mailService = new EmailService();
                ActiveQuoteService _activeQuote = new ActiveQuoteService();
                List<CalTagViewModel> lstCalTagViewModel = _activeQuote.GetQuoteByCTStatus();
                FileCreationService _fileCreation = new FileCreationService();
                List<string> lstFilePathWithNames = new List<string>();

                if (lstCalTagViewModel.Count > 0)
                {
                    string filePath = _fileCreation.GenerateCTFlatFiles(lstCalTagViewModel);
                    if (!string.IsNullOrEmpty(filePath))
                        lstFilePathWithNames.Add(filePath);
                }
                if (lstFilePathWithNames.Count > 0)
                {
                    _activeQuote.ChangeCTStatus();
                    mailService.SendMail("CallTagInfo", "Please find the attached CallTag files", false, true, lstFilePathWithNames, null, ConfigurationManager.AppSettings["fromAddress"], ConfigurationManager.AppSettings["WareHouseEmail"]);
                }

            }
            catch (Exception ex)
            {
                Elmah.SqlErrorLog ErrorLog = new Elmah.SqlErrorLog(conn);
                ErrorLog.Log(new Elmah.Error(new Exception(ex.ToString())));
            }
        }

        #endregion

        #region Automated Email - Decision Wizard

        private void ScheduleDWEmailReminder(object state)
        {
            try
            {
                SetDWEmailReminderTimer();

                ActiveQuoteService _activeQuote = new ActiveQuoteService();
                List<EmailTemplateViewModel> lstEmailTEmailTemplateModel = _activeQuote.GetDwTemplateEmail();

                EmailBodyGeneratorService emailService = new EmailBodyGeneratorService();


                foreach (EmailTemplateViewModel item in lstEmailTEmailTemplateModel)
                {
                    int[] dwRemianderDays = ConfigurationManager.AppSettings["DwRemainderDays"].Split(',').Select(e => Convert.ToInt32(e)).ToArray(); // Dw Remainder Days
                    List<int> lstDwRemianderDays = ConfigurationManager.AppSettings["DwRemainderDays"].Split(',').Select(e=> Convert.ToInt32(e)).ToList();
                    //int days = item.EmailDWTemplateList != null && item.EmailDWTemplateList.Where(e => e.NoOfDays == 0 || e.NoOfDays == 4 || e.NoOfDays == 10 || e.NoOfDays == 19 || e.NoOfDays == 45 || e.NoOfDays == 60).FirstOrDefault() != null ?
                    //       item.EmailDWTemplateList.Where(e => e.NoOfDays == 0 || e.NoOfDays == 4 || e.NoOfDays == 10 || e.NoOfDays == 19 || e.NoOfDays == 45 || e.NoOfDays == 60).OrderBy(e => e.NoOfDays).FirstOrDefault().NoOfDays < 0 ? 0 :
                    //       item.EmailDWTemplateList.Where(e => e.NoOfDays == 0 || e.NoOfDays == 4 || e.NoOfDays == 10 || e.NoOfDays == 19 || e.NoOfDays == 45 || e.NoOfDays == 60).OrderBy(e => e.NoOfDays).FirstOrDefault().NoOfDays :
                    //       item.EmailDWTemplateList.FirstOrDefault().NoOfDays;

                    int days = item.EmailDWTemplateList != null && item.EmailDWTemplateList.Where(e => lstDwRemianderDays.Contains(e.NoOfDays)).FirstOrDefault() != null ?
                           item.EmailDWTemplateList.Where(e => lstDwRemianderDays.Contains(e.NoOfDays)).OrderBy(e => e.NoOfDays).FirstOrDefault().NoOfDays < 0 ? 0 :
                           item.EmailDWTemplateList.Where(e => lstDwRemianderDays.Contains(e.NoOfDays)).OrderBy(e => e.NoOfDays).FirstOrDefault().NoOfDays :
                           item.EmailDWTemplateList.FirstOrDefault().NoOfDays;

                  

                    string templateType = item.EmailDWTemplateList.FirstOrDefault().IsActive == true ? "Active" : "Idle";
                    string subject = ConfigurationManager.AppSettings["EmailSubjectLine"];//"DW reminder-";

                    try
                    {
                        #region DwRemainder
                        bool remainderDayExits = Array.IndexOf(dwRemianderDays, days) > -1 ? true : false;

                        if (remainderDayExits)
                        {
                            string dwEmailType = ConfigurationManager.AppSettings["DwRemainderText"] + " " + days + " WEB";
                            emailService.InitialiseHTMLParser(item, templateType, subject, dwEmailType);
                        }

                        #endregion

                        //switch (days)
                        //{
                        //    //case 0:
                        //    //    emailService.InitialiseHTMLParser(item, "initial", subject + "initial");
                        //    //    break;
                        //    //case 4:
                        //    //    emailService.InitialiseHTMLParser(item, templateType, subject, Resources.DWEmailType.Reminder4);
                        //    //    break;
                        //    //case 10:
                        //    //    emailService.InitialiseHTMLParser(item, templateType, subject, Resources.DWEmailType.Reminder10);
                        //    //    break;
                        //    //case 19:
                        //    //    emailService.InitialiseHTMLParser(item, templateType, subject, Resources.DWEmailType.Reminder19);
                        //    //    break;
                        //    //case 45:
                        //    //    emailService.InitialiseHTMLParser(item, "outstanding", subject, Resources.DWEmailType.Reminder45);
                        //    //    break;
                        //    //case 60:
                        //    //    emailService.InitialiseHTMLParser(item, "outstanding", subject, Resources.DWEmailType.Reminder60);
                        //    //    break;
                        //    default:
                        //        break;
                        //}
                    }
                    catch (Exception ex)
                    {
                        Elmah.SqlErrorLog ErrorLog = new Elmah.SqlErrorLog(conn);
                        ErrorLog.Log(new Elmah.Error(new Exception(ex.ToString())));
                    }
                }

            }
            catch (Exception ex)
            {
                Elmah.SqlErrorLog ErrorLog = new Elmah.SqlErrorLog(conn);
                ErrorLog.Log(new Elmah.Error(new Exception(ex.ToString())));
                SetDWEmailReminderTimer();
            }


        }

        #endregion

        #region Service Scheduler

        private void SetCallTagTimer()
        {
            try
            {
                System.Threading.Timer tpcTimer;

                string[] time = ConfigurationManager.AppSettings["ScheduleCallTagAt"].Trim().Split(':');

                // trigger the event at specified time
                DateTime requiredTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Convert.ToInt32(time[0]), Convert.ToInt32(time[1]), 00);// preferred hour 20,// preferred minute

                if (DateTime.Now >= requiredTime)
                {
                    requiredTime = requiredTime.AddDays(1);
                }

                // initialize timer only, do not specify the start time or the interval
                tpcTimer = new System.Threading.Timer(new TimerCallback(ExecuteTask));
                // first parameter is the start time and the second parameter is the interval
                // Timeout.Infinite means do not repeat the interval, only start the timer
                tpcTimer.Change((int)(requiredTime - DateTime.Now).TotalMilliseconds, Timeout.Infinite);

                GenerateCallTagLog(requiredTime);
            }
            catch (Exception ex)
            {
                Elmah.SqlErrorLog ErrorLog = new Elmah.SqlErrorLog(conn);
                ErrorLog.Log(new Elmah.Error(new Exception(ex.ToString())));
            }

        }

        private void SetDWEmailReminderTimer()
        {
            try
            {
                System.Threading.Timer tpcTimer;

                string[] time = ConfigurationManager.AppSettings["ScheduleDWAt"].Trim().Split(':');

                // trigger the event at specified time
                DateTime requiredTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Convert.ToInt32(time[0]), Convert.ToInt32(time[1]), 00);// preferred hour 20,// preferred minute

                if (DateTime.Now >= requiredTime)
                {
                    requiredTime = requiredTime.AddDays(1);
                }


                // initialize timer only, do not specify the start time or the interval
                tpcTimer = new System.Threading.Timer(new TimerCallback(ScheduleDWEmailReminder));
                // first parameter is the start time and the second parameter is the interval
                // Timeout.Infinite means do not repeat the interval, only start the timer
                tpcTimer.Change((int)(requiredTime - DateTime.Now).TotalMilliseconds, Timeout.Infinite);

                GenerateDWReminderLog(requiredTime);
            }
            catch (Exception ex)
            {
                Elmah.SqlErrorLog ErrorLog = new Elmah.SqlErrorLog(conn);
                ErrorLog.Log(new Elmah.Error(new Exception(ex.ToString())));
            }
        }

        private void ExecuteTask(object e)
        {
            SetCallTagTimer();
            GenerateFlatFiles();
        }

        #endregion


        private void GenerateDWReminderLog(DateTime scheduledTime)
        {
            try
            {
                FileStream fs = new FileStream(ConfigurationManager.AppSettings["CTFlatFile"] + "CTDWReminder.txt", FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter m_streamWriter = new StreamWriter(fs);
                m_streamWriter.BaseStream.Seek(0, SeekOrigin.End);
                m_streamWriter.WriteLine("DW Scheduled at: " + scheduledTime);
                m_streamWriter.Flush();
                m_streamWriter.Close();
            }
            catch (Exception ex)
            {
                Elmah.SqlErrorLog ErrorLog = new Elmah.SqlErrorLog(conn);
                ErrorLog.Log(new Elmah.Error(new Exception(ex.ToString())));
            }
        }

        private void GenerateCallTagLog(DateTime scheduledTime)
        {
            try
            {
                FileStream fsCallTag = new FileStream(ConfigurationManager.AppSettings["CTFlatFile"] + "CTSCheduler.txt", FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter m_CallTagstreamWriter = new StreamWriter(fsCallTag);
                m_CallTagstreamWriter.BaseStream.Seek(0, SeekOrigin.End);
                m_CallTagstreamWriter.WriteLine("CallTag Scheduled at: " + scheduledTime);
                m_CallTagstreamWriter.Flush();
                m_CallTagstreamWriter.Close();
            }
            catch (Exception ex)
            {
                Elmah.SqlErrorLog ErrorLog = new Elmah.SqlErrorLog(conn);
                ErrorLog.Log(new Elmah.Error(new Exception(ex.ToString())));
            }
        }

    }
}
