using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using EdgeInfo.DataItems;
using WheelsScraper;

namespace EdgeInfo.Helpers
{
    class RequestHelper
    {
        public static string DownloadLatesFile(string login, string password, string host, string localFileName)
        {
            List<FtpFile> fileList = new List<FtpFile>();

            if (!host.StartsWith("ftp://"))
                host = "ftp://" + host;

            try
            {
                var ftpRequest = (FtpWebRequest)FtpWebRequest.Create(host);
                {
                    ftpRequest.Method = WebRequestMethods.Ftp.ListDirectory;
                    ftpRequest.Credentials = new NetworkCredential(login, password);

                    StreamReader reader = new StreamReader(ftpRequest.GetResponse().GetResponseStream());

                    while (!reader.EndOfStream)
                    {
                        string ftpLine = reader.ReadLine();

                        if (ftpLine.ToLower().Contains(".csv"))
                        {
                            var ftpRequestForDateTime = (FtpWebRequest)FtpWebRequest.Create(host + "/" + ftpLine);
                            {
                                ftpRequestForDateTime.Method = WebRequestMethods.Ftp.GetDateTimestamp;
                                ftpRequestForDateTime.Credentials = new NetworkCredential(login, password);
                                FtpWebResponse response = (FtpWebResponse)ftpRequestForDateTime.GetResponse();
                                DateTime resp = response.LastModified;

                                FtpFile fileInfo = new FtpFile
                                {
                                    FileName = ftpLine,
                                    Date = resp
                                };

                                fileList.Add(fileInfo);
                            }
                        }
                    }
                    string theNewestFile = string.Empty;
                    int count = 0;
                    foreach (FtpFile file in fileList)
                    {
                        if (count != fileList.Count - 1)
                        {
                            count++;
                            if (file.Date < fileList[count].Date)
                            {
                                theNewestFile = fileList[count].FileName;
                            }
                            else
                            {
                                theNewestFile = file.FileName;
                            }
                        }
                    }

                    if (theNewestFile != string.Empty)
                    {
                        using (WebClient requestForDownload = new WebClient())
                        {
                            requestForDownload.Credentials = new NetworkCredential(login, password);

                            byte[] fileData = requestForDownload.DownloadData(host + "/" + theNewestFile);

                            using (FileStream file = File.Create(localFileName))
                            {
                                file.Write(fileData, 0, fileData.Length);
                                file.Close();
                            }
                        }

                        return localFileName;
                    }

                }

                return string.Empty;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
