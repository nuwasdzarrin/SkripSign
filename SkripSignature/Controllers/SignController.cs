using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using SignatureData;
using SkripSignature.SignProcess;

namespace SkripSignature.Controllers
{
    public class SignController : ApiController
    {
        string pathToFiles = HttpContext.Current.Server.MapPath("~/UploadFile/");
        public HttpResponseMessage Post([FromBody] SignatureTable signature)
        {
            try
            {
                Cert myCert = null;
                try
                {
                    myCert = new Cert(pathToFiles + "sertifikat/" + signature.certName, signature.password);
                }
                catch (Exception ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }

                //Adding Meta Datas
                MetaData MyMD = new MetaData();
                MyMD.Author = signature.author;
                MyMD.Title = signature.title;
                MyMD.Subject = signature.subject;
                MyMD.Keywords = signature.keyword;

                PDFSigner pdfs = new PDFSigner(pathToFiles + "input/" + signature.pdfName, pathToFiles + "output/sign_" + signature.pdfName, myCert, MyMD);
                pdfs.Sign(signature.reason, signature.email, signature.location, true);
                using (SignatureDBEntities entities = new SignatureDBEntities())
                {
                    entities.SignatureTables.Add(signature);
                    entities.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, signature);
                    message.Headers.Location = new Uri(Request.RequestUri + "/" + signature.id.ToString());
                    return message;
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
