using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using EbayNewPartsListingInfo.DataItems;
using WheelsScraper;

namespace EbayNewPartsListingInfo.Helpers
{
    public class FTPManager
    {
        private IMessagePrinter messagePrinter;

        public FTPManager(IMessagePrinter messagePrinter)
        {
            this.messagePrinter = messagePrinter;
        }

        public string DownloadFile(string login, string password, string host)
        {
            //List<string> fileList = new List<string>();
            List<FtpFile> fileList = new List<FtpFile>();
            string fileName = "dataFTP.csv";
            string inputFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);

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

                        if (ftpLine.Contains(".csv"))
                        {
                            var ftpRequestForDateTime = (FtpWebRequest)FtpWebRequest.Create(host + "/" + ftpLine);
                            {
                                ftpRequestForDateTime.Method = WebRequestMethods.Ftp.GetDateTimestamp;
                                ftpRequestForDateTime.Credentials = new NetworkCredential(login, password);
                                FtpWebResponse response = (FtpWebResponse)ftpRequestForDateTime.GetResponse();
                                DateTime resp = response.LastModified;

                                FtpFile fileInfo = new FtpFile
                                {
                                    Name = ftpLine,
                                    DateTime = resp
                                };

                                fileList.Add(fileInfo);
                            }
                        }
                    }
                    string theNewestFile = string.Empty;
                    string currentFile = string.Empty;
                    int count = 0;
                    foreach (FtpFile file in fileList)
                    {
                        if (count != fileList.Count - 1)
                        {
                            count++;
                            if (file.DateTime < fileList[count].DateTime)
                            {
                                theNewestFile = fileList[count].Name;
                            }
                            else
                            {
                                theNewestFile = file.Name;
                            }
                        }
                    }

                    if (theNewestFile != string.Empty)
                    {
                        using (WebClient requestForDownload = new WebClient())
                        {
                            requestForDownload.Credentials = new NetworkCredential(login, password);

                            byte[] fileData = requestForDownload.DownloadData(host + "/" + theNewestFile);

                            using (FileStream file = File.Create(inputFilePath))
                            {
                                file.Write(fileData, 0, fileData.Length);
                                file.Close();
                            }
                        }

                        return inputFilePath;
                    }

                }

                return string.Empty;
            }
            catch (Exception e)
            {
                messagePrinter.PrintMessage(e.Message, ImportanceLevel.Critical);
                return null;
            }
        }
    }
}
