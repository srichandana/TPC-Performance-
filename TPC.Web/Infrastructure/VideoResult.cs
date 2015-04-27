using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Hosting;
using System.Web.Mvc;
using System.IO;
using System.Configuration;

namespace TPC.Web.Infrastructure
{
    public class VideoResult: ActionResult
    {
        
        /// The below method will respond with the Video file 
        public override void ExecuteResult(ControllerContext context) 
        { 
            //The File Path 
            string filePath = ConfigurationManager.AppSettings["CommonRepository"];
            string fileName = ConfigurationManager.AppSettings["helpFileName"];

            var videoFilePath = filePath + fileName;
            //The header information 
            context.HttpContext.Response.AddHeader("Content-Disposition", "attachment; filename=How to use your Decision Wizard");
            var file = new FileInfo(videoFilePath); 
            //Check the file exist,  it will be written into the response 
            if (file.Exists)
            {
                var stream = file.OpenRead(); 
                var bytesinfile = new byte[stream.Length]; 
                stream.Read(bytesinfile, 0, (int)file.Length); 
                context.HttpContext.Response.BinaryWrite(bytesinfile); 
            } 
        } 
    } 
}
   