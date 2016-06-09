using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using System.Web;
using System.IO;
using System.Net.Http.Headers;
using System.Web.Http.Cors;
using System.Configuration;
using ADPQ.Business.Business;

namespace MicroService.Controller
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class FileUploadController : ApiController
    {
        public async Task<List<string>> PostAsync([FromUri]Guid Token, [FromUri]Guid PER_ID)
        {
            ADPQ.Business.Business.CommonBusiness ObjCommon = new ADPQ.Business.Business.CommonBusiness();
            try
            {
                if (Request.Content.IsMimeMultipartContent())
                {
                    string UploadPath =(Token !=Guid.Empty && PER_ID!=Guid.Empty)? ConfigurationManager.AppSettings["UploadPath"] + PER_ID.ToString(): ConfigurationManager.AppSettings["UploadPath"];

                    if (!System.IO.Directory.Exists(UploadPath))
                        System.IO.Directory.CreateDirectory(UploadPath);

                    MyStreamProvider streamProvider = new MyStreamProvider(UploadPath);

                    await Request.Content.ReadAsMultipartAsync(streamProvider);

                    List<string> messages = new List<string>();
                    foreach (var file in streamProvider.FileData)
                    {                       
                        FileInfo fi = new FileInfo(file.LocalFileName);
                        string newFileName = UploadPath + "\\" + Guid.NewGuid() + Path.GetExtension(fi.FullName);
                        System.IO.File.Move(fi.FullName, newFileName);
                        if (file.Headers.ContentType.MediaType.Equals("image/png") || file.Headers.ContentType.MediaType.Equals("image/jpg") || file.Headers.ContentType.MediaType.Equals("image/jpeg"))
                        {                         
                            messages.Add(ObjCommon.getImage(PER_ID, Path.GetFileName(newFileName), file.Headers.ContentType.MediaType));
                            return messages;
                        }                      
                        messages.Add(Path.GetFileName(newFileName));
                    }

                    return messages;
                }
                else
                {
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid Request!");
                    throw new HttpResponseException(response);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
           
        }

        [HttpGet]
        public HttpResponseMessage Get(Guid token, Guid id, [FromUri]String filename)
        {
            UserBusiness ObjUser = new UserBusiness();
            ChildBusiness ObjChild = new ChildBusiness();
            bool IsSessionActive = ObjUser.ValidateToken(token);
            if (IsSessionActive)
            {
                string path = (token != Guid.Empty && id != Guid.Empty) ? ConfigurationManager.AppSettings["UploadPath"] + id.ToString() + "\\" + filename : ConfigurationManager.AppSettings["UploadPath"]+"\\"+ filename;
                if (File.Exists(path))
                {

                    HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                    var stream = new FileStream(path, FileMode.Open);
                    result.Content = new StreamContent(stream);
                    result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                    result.Content.Headers.ContentDisposition.FileName = filename;
                    return result;
                }
                else
                {
                    return new HttpResponseMessage(HttpStatusCode.NotFound);
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new { result = false });
            }

        }
    }

   

    public class MyStreamProvider : MultipartFormDataStreamProvider
    {
        public MyStreamProvider(string uploadPath)
            : base(uploadPath)
        {

        }

        public override string GetLocalFileName(HttpContentHeaders headers)
        {
            string fileName = headers.ContentDisposition.FileName;
            if (string.IsNullOrWhiteSpace(fileName))
            {
                fileName = Guid.NewGuid().ToString() + ".data";
            }
            return fileName.Replace("\"", string.Empty);
        }
    }
}
