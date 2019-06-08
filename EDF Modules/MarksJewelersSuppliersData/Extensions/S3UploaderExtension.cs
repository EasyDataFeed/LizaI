using Amazon;
using Amazon.S3;
using Amazon.S3.IO;
using Scraper.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarksJewelersSuppliersData.Extensions
{
    public static class EnumerableExtension
    {
        public static List<string> GetAmazonFoolderFiles(this List<string> source, S3Settings settings)
        {
            List<string> files = new List<string>();

            AmazonS3Client s3Client = new AmazonS3Client(settings.AccessKeyID, settings.SecretAccessKey, RegionEndpoint.USEast1);
            S3DirectoryInfo dir = new S3DirectoryInfo(s3Client, settings.BucketName, settings.Folder);
            foreach (IS3FileSystemInfo file in dir.GetFileSystemInfos())
                files.Add(file.Name.TrimStart('@'));

            return files;
        }
    }
}
