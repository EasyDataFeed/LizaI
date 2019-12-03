using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace MarksJewelersFtpData.Helper_Methods
{
    class RequestHelper
    {
        public static Dictionary<string, string> DownloadImage(string url, string partNumber, string fileName)
        {
            try
            {
                using (var client = new WebClient())
                {
                    client.DownloadFile(url, FileHelper.GetSettingsPath(fileName, partNumber));
                }
            }
            catch { }

            return new Dictionary<string, string>() { { FileHelper.GetSettingsPath(fileName, partNumber), partNumber } };
        }

        public static void UploadFtpFile(string filePath, string folderPath, string ftpUsername, string ftpPassword)
        {

            FtpWebRequest request;
            try
            {
                string absoluteFileName = Path.GetFileName(filePath);

                request = WebRequest.Create(new Uri($@"ftp://{"ftp.sirv.com"}/{folderPath}/{absoluteFileName}")) as FtpWebRequest;
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.UseBinary = true;
                request.UsePassive = true;
                request.KeepAlive = true;
                request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);
                request.ConnectionGroupName = "group";

                using (FileStream fs = File.OpenRead(filePath))
                {
                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                    fs.Close();
                    Stream requestStream = request.GetRequestStream();
                    requestStream.Write(buffer, 0, buffer.Length);
                    requestStream.Flush();
                    requestStream.Close();
                }
            }
            catch (Exception ex)
            {

            }
        }

        public static void FtpCreateFolder(string ftpAddress,  string folderName, string ftpUName, string ftpPWord)
        {
            try
            {
                WebRequest ftpRequest = WebRequest.Create("ftp://" + ftpAddress + "/" + folderName);
                ftpRequest.Method = WebRequestMethods.Ftp.MakeDirectory;
                ftpRequest.Credentials = new NetworkCredential(ftpUName, ftpPWord);

                using (var resp = (FtpWebResponse)ftpRequest.GetResponse()) { }
            }
            catch { }
        }

        public static void FtpDeleteFolder(string ftpAddress, string supplier, string partNumber, string ftpUName, string ftpPWord)
        {
            try
            {
                FtpWebRequest ftpRequest = WebRequest.Create("ftp://" + ftpAddress + "/" + partNumber) as FtpWebRequest;
                // Credentials and login handling...

                ftpRequest.Method = WebRequestMethods.Ftp.RemoveDirectory;
                ftpRequest.Credentials = new NetworkCredential(ftpUName, ftpPWord);

                string result = string.Empty;
                FtpWebResponse response = (FtpWebResponse)ftpRequest.GetResponse();
                long size = response.ContentLength;
                Stream datastream = response.GetResponseStream();
                StreamReader sr = new StreamReader(datastream);
                result = sr.ReadToEnd();
                sr.Close();
                datastream.Close();
                response.Close();
            }
            catch { }
        }

        public static List<string> FtpDirectoryListing(string path, string serverAdress, string login, string password)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + serverAdress + path);
            request.Credentials = new NetworkCredential(login, password);

            request.Method = WebRequestMethods.Ftp.ListDirectory;

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);

            List<string> result = new List<string>();

            while (!reader.EndOfStream)
            {
                result.Add(reader.ReadLine());
            }

            reader.Close();
            response.Close();

            return result;
        }

        public static void FtpDeleteFile(string path, string serverAdress, string login, string password)
        {
            FtpWebRequest clsRequest = (System.Net.FtpWebRequest)WebRequest.Create("ftp://" + serverAdress + path);
            clsRequest.Credentials = new System.Net.NetworkCredential(login, password);

            clsRequest.Method = WebRequestMethods.Ftp.DeleteFile;

            string result = string.Empty;
            FtpWebResponse response = (FtpWebResponse)clsRequest.GetResponse();
            long size = response.ContentLength;
            Stream datastream = response.GetResponseStream();
            StreamReader sr = new StreamReader(datastream);
            result = sr.ReadToEnd();
            sr.Close();
            datastream.Close();
            response.Close();
        }

        public static void FtpDeleteDirectory(string path, string serverAdress, string login, string password)
        {
            if (!serverAdress.EndsWith("/"))
                serverAdress += "/";

            if (!path.EndsWith("/"))
                path += "/";

            FtpWebRequest clsRequest = (System.Net.FtpWebRequest)WebRequest.Create("ftp://" + serverAdress + path);
            clsRequest.Credentials = new System.Net.NetworkCredential(login, password);

            List<string> filesList = FtpDirectoryListing(path, serverAdress, login, password);

            foreach (string file in filesList)
            {
                FtpDeleteFile(path + file, serverAdress, login, password);
            }

            clsRequest.Method = WebRequestMethods.Ftp.RemoveDirectory;

            string result = string.Empty;
            FtpWebResponse response = (FtpWebResponse)clsRequest.GetResponse();
            long size = response.ContentLength;
            Stream datastream = response.GetResponseStream();
            StreamReader sr = new StreamReader(datastream);
            result = sr.ReadToEnd();
            sr.Close();
            datastream.Close();
            response.Close();
        }
    }
}
