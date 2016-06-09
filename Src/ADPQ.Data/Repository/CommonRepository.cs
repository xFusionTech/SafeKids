using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADPQ.Data.Repository
{
   public class CommonRepository
    {
        public string getImage(Guid PER_ID,Guid ChildId, string filename, string MediaType)
        {
            string UploadPath = ConfigurationManager.AppSettings["UploadPath"] + PER_ID.ToString();

            byte[] imageArray = System.IO.File.ReadAllBytes(UploadPath + "\\" + filename);
            string base64ImageRepresentation = Convert.ToBase64String(imageArray);
            return "data:" + MediaType + ";base64," + base64ImageRepresentation;
        }

        public string getImage(Guid PER_ID,string filename, string MediaType)
        {
            string UploadPath = ConfigurationManager.AppSettings["UploadPath"] + PER_ID.ToString();

            byte[] imageArray = System.IO.File.ReadAllBytes(UploadPath + "\\" + filename);
            string base64ImageRepresentation = Convert.ToBase64String(imageArray);
            return "data:" + MediaType + ";base64," + base64ImageRepresentation;
        }
    }
}
