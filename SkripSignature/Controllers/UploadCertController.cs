using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using SkripSignature.UploadRepo;

namespace SkripSignature.Controllers
{
    public class UploadCertController : ApiController
    {
        // asynchronous function 
        [Mime]
        public async Task<FileUploadDetails> Post()
        {
            // file path
            var fileuploadPath = HttpContext.Current.Server.MapPath("~/UploadFile/sertifikat");

            // 
            var multiFormDataStreamProvider = new MultiFileUploadProvider(fileuploadPath);

            // Read the MIME multipart asynchronously 
            await Request.Content.ReadAsMultipartAsync(multiFormDataStreamProvider);

            string uploadingFileName = multiFormDataStreamProvider
                .FileData.Select(x => x.LocalFileName).FirstOrDefault();

            // Create response, assigning appropriate values to properties 
            return new FileUploadDetails
            {
                FilePath = uploadingFileName,

                FileName = Path.GetFileName(uploadingFileName),

                FileLength = new FileInfo(uploadingFileName).Length,

                FileCreatedTime = DateTime.Now.ToLongDateString()
            };
        }
    }
}
