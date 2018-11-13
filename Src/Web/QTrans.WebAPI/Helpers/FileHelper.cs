using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.IO;
using QTrans.Logging;
using QTrans.Models;
using QTrans.Repositories;
using QTrans.WebAPI.Models;

namespace QTrans.WebAPI.Helpers
{
    public class FileHelper
    {
        public static string FilePath = ConfigurationManager.AppSettings["FilePath"].ToString();
        public static string FileURL = ConfigurationManager.AppSettings["FileURL"].ToString();

        public static Int64 StoreFile(string FileType, Int64 UserId, HttpRequest httpRequest)
        {
            string result = string.Empty;
            Int64 FileID = 0;
            try
            {
                if (httpRequest.Files != null && httpRequest.Files.Count > 0)
                {
                    HttpPostedFile file = httpRequest.Files[0];
                    string OriginalFileName = file.FileName;
                    string NewFileName = Guid.NewGuid().ToString().ToUpperInvariant() + Path.GetExtension(file.FileName);

                    if (!Directory.Exists(FilePath))
                        Directory.CreateDirectory(FilePath);

                    string filePath = FilePath + NewFileName;
                    file.SaveAs(filePath);

                    //var result = DBContext.ExecuteScalar("API_File_Save",
                    //    new SqlParameter("fileName", SqlDbType.NVarChar, 50) { Value = NewFileName },
                    //    new SqlParameter("originalFileName", SqlDbType.NVarChar, 250) { Value = OriginalFileName },
                    //    new SqlParameter("fileType", SqlDbType.NVarChar, 10) { Value = FileType },
                    //    new SqlParameter("comment", SqlDbType.NVarChar, 1000) { Value = string.IsNullOrEmpty(Comments) ? "" : Comments },
                    //    new SqlParameter("userId", SqlDbType.BigInt) { Value = UserId }
                    //);

                    //return result == null ? string.Empty : result.ToString();
                }

                return FileID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string RetriveFilePath(string FileName)
        {
            string path = FilePath + FileName;

            if (File.Exists(path))
                return FileURL + FileName;
            else
                return string.Empty;
        }

        //public static string GetFileURLByID(string fileId)
        //{
        //    string fileName = DBContext.ExecuteScalar("API_File_GetFileName",
        //               new SqlParameter("fileId", SqlDbType.NVarChar, 36) { Value = fileId }
        //           ).ToString();

        //    return FileURL + fileName;
        //}
    }
}