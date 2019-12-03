using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Kravet.Helpers
{
    public class FTPHelper
    {
        public static string DownloadFtpFile(string login, string password, string host, string ftpFilePath, string localFilePath)
        {
            try
            {
                if (!host.EndsWith("/"))
                    host += "/";

                if (ftpFilePath.StartsWith("/"))
                    ftpFilePath = ftpFilePath.TrimStart('/');

                var ftpRequest = (FtpWebRequest)FtpWebRequest.Create(host + ftpFilePath);
                {
                    ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;
                    ftpRequest.Credentials = new NetworkCredential(login, password);
                    FtpWebResponse responseFileDownload = (FtpWebResponse)ftpRequest.GetResponse();

                    Stream responseStream = responseFileDownload.GetResponseStream();
                    FileStream writeStream = new FileStream(localFilePath, FileMode.Create);

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

                    ftpRequest = null;
                    responseFileDownload = null;

                    return localFilePath;
                }
            }
            catch (Exception e)
            {
                
                return string.Empty;
            }
        }
    }
}
