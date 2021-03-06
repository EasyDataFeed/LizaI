﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Turn14ApiScraper.Helpers
{
    class RequestHelper
    {
        public static string DownloadFTPFile(string localPath, string ftp, string fileName, string user, string password)
        {
            try
            {
                FtpWebRequest requestFileDownload = (FtpWebRequest)WebRequest.Create(ftp + "/" + fileName);
                requestFileDownload.Credentials = new NetworkCredential(user, password);
                requestFileDownload.Method = WebRequestMethods.Ftp.DownloadFile;

                FtpWebResponse responseFileDownload = (FtpWebResponse)requestFileDownload.GetResponse();

                Stream responseStream = responseFileDownload.GetResponseStream();
                FileStream writeStream = new FileStream(localPath, FileMode.Create);

                int Length = 2048;
                byte[] buffer = new byte[Length];
                int bytesRead = responseStream.Read(buffer, 0, Length);

                while (bytesRead > 0)
                {
                    writeStream.Write(buffer, 0, bytesRead);
                    bytesRead = responseStream.Read(buffer, 0, Length);
                }

                responseStream.Close();
                writeStream.Close();
                return localPath;
            }
            catch (Exception e)
            {
                return $"Error: {e.Message}";
            }
        }
    }
}
