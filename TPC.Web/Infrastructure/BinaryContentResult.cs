using System.IO;
using System.Web;
using System.Web.Mvc;
using TPC.Web.Infrastructure;

namespace TPC.Web.Infrastructure
{
    public class BinaryContentResult :ActionResult
    {
        private readonly string _contentType;
        private readonly byte[] _contentBytes;

        public BinaryContentResult(byte[] contentBytes, string contentType)
        {
            this._contentBytes = contentBytes;
            this._contentType = contentType;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;
            response.Clear();
            response.Cache.SetCacheability(HttpCacheability.Public);
            response.ContentType = this._contentType;

            using (var stream = new MemoryStream(this._contentBytes))
            {
                stream.WriteTo(response.OutputStream);
                stream.Flush();
            }
        }
    }
}