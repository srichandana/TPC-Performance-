using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using TPC.Common.Enumerations;
using TPC.Context;
using TPC.Context.EntityModel;
using TPC.Context.Interfaces;
using TPC.Core.Infrastructure;
using TPC.Core.Interfaces;
using TPC.Core.Models;
using TPC.Core.Models.Models;
using TPC.Core.Models.ViewModels;
using System.Configuration;

namespace TPC.Core
{
    public class LibrarianResourcesService : ServiceBase<LibrarianResourcesViewModel>, ILibrarianResourcesService
    {

        public LibrarianResourcesViewModel GetLibraryResource()
        {
            LibrarianResourcesViewModel LRVM = new LibrarianResourcesViewModel();
            QuoteViewService qvs = new QuoteViewService();
            LRVM.DictlibraryResource = new Dictionary<string, Dictionary<string, string>>();
            string dir = ConfigurationManager.AppSettings["CommonRepository"] + ConfigurationManager.AppSettings["LibraryResourseRootDirectory"];
            DirSearch(dir, LRVM);
            //foreach(string folderName in LRVM.DictlibraryResource.Keys)
            //{
            //    string path = LRVM.DictlibraryResource[folderName].Keys.Select(e => LRVM.DictlibraryResource[folderName][e] == "MainImage" ? e : string.Empty).FirstOrDefault();
            //}
            LRVM.UserVM = UserVM;
            qvs.UserVM = UserVM;
            if (UserVM != null)
            {
                LRVM.UserVM.CurrentQuoteID = qvs.getCustomerSCQuoteID();
            }
            return LRVM;
        }

        private void DirSearch(string sDir, LibrarianResourcesViewModel lrvm)
        {

            try
            {
                foreach (string d in Directory.GetDirectories(sDir))
                {
                    string folderName = Path.GetFileName(d);
                    string rootDirName = Path.GetFileName(Directory.GetParent(d).FullName);
                    string currentKey = rootDirName == ConfigurationManager.AppSettings["LibraryResourseRootDirectoryName"] ? folderName : rootDirName;
                    if (lrvm.DictlibraryResource.Keys.Where(e => e == currentKey).FirstOrDefault() == null)
                        lrvm.DictlibraryResource.Add(currentKey, new Dictionary<string, string>());
                    foreach (string f in Directory.GetFiles(d))
                    {
                        string ext = Path.GetExtension(f);

                        if (ext == ".PNG" || ext == ".png")
                        {
                            lrvm.DictlibraryResource[currentKey].Add(f, "thumbnail");
                        }
                        else if (ext == ".jpg")
                        {
                            lrvm.DictlibraryResource[currentKey].Add(f, "MainImage");

                        }
                        else if (ext == ".pdf")
                        {
                            lrvm.DictlibraryResource[currentKey].Add(f, "DocumentPdf");
                        }
                    }
                    DirSearch(d, lrvm);
                }
            }
            catch (System.Exception excpt)
            {
                Console.WriteLine(excpt.Message);
            }
        }


    }
}
