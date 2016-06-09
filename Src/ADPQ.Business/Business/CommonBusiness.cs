using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADPQ.Business.Business
{
  public class CommonBusiness
    {
   
        public string getImage(Guid PER_ID, string filename,string MediaType )
        {
            string UploadPath = (PER_ID!=Guid.Empty)?ConfigurationManager.AppSettings["UploadPath"] + PER_ID.ToString(): ConfigurationManager.AppSettings["UploadPath"];

            byte[] imageArray = System.IO.File.ReadAllBytes(UploadPath+"\\"+ filename);
            string base64ImageRepresentation = Convert.ToBase64String(imageArray);
            return "data:"+ MediaType+";base64,"+ base64ImageRepresentation + " |~|"+ filename;
        }
    }
}
