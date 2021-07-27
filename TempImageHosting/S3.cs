using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Amazon;
using Amazon.S3;
using Amazon.S3.Model;

namespace TempImageHosting
{
    public class S3
    {
        private readonly AmazonS3Client _client;
        private const string _bucketName = "test-image-upload-test";
        private const string _baseUrl = "https://test-image-upload-test.s3.eu-west-1.amazonaws.com/";
        private readonly RegionEndpoint _bucketRegion = RegionEndpoint.EUWest1;

        public S3()
        {
            _client = new AmazonS3Client(
                Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID"),
                Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY"),
                _bucketRegion);
        }

        public async Task<List<string>> ListObjectsAsync()
        {
            var response = await _client.ListObjectsAsync(_bucketName);

            List<string> images = new();

            foreach (var s3Object in response.S3Objects)
            {
                string objectUrl = $"{_baseUrl}{s3Object.Key}";

                images.Add(objectUrl);
            }

            return images;
        }

        public string GenerateUploadUrl(string fileName)
        { 
            string key = GenerateObjectKey(fileName);

            var uploadedUrl = _client.GetPreSignedURL(new GetPreSignedUrlRequest
            {
                BucketName = _bucketName,
                Key = key,
                Expires = DateTime.Now.AddSeconds(60),
                Verb = HttpVerb.PUT,
                //ContentType = "image/jpeg",
                
            });

            return uploadedUrl;
        }

        private static string GenerateObjectKey(string fileName)
        {
            string randomId = Guid.NewGuid().ToString();
            string date = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");

            return $"{date}-{randomId}.{fileName}";
        }

    }
}
