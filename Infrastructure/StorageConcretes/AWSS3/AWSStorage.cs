using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Application.ExceptionTypes;
using Application.StorageInterfaces.AWS;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.StorageConcretes.AWSS3
{
   

    public class AWSStorage : Storage, IAWSStorage
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;

        //public AWSStorage(IConfiguration configuration)
        //{
        //    _s3Client = new AmazonS3Client(
        //        configuration["AWS:AccessKeyId"],
        //        configuration["AWS:SecretAccessKey"],
        //        Amazon.RegionEndpoint.GetBySystemName(configuration["AWS:Region"]));

        //    _bucketName = configuration["AWS:BucketName"];
        //}
        public AWSStorage(IConfiguration configuration)
        {
            string accessKeyId = configuration["AWS:AccessKeyId"]
                ?? throw new OperationException("AWS AccessKeyId is not configured.");
            string secretAccessKey = configuration["AWS:SecretAccessKey"]
                ?? throw new OperationException("AWS SecretAccessKey is not configured.");
            string region = configuration["AWS:Region"]
                ?? throw new OperationException("AWS Region is not configured.");
            _bucketName = configuration["AWS:BucketName"]
                ?? throw new OperationException("AWS BucketName is not configured.");

            _s3Client = new AmazonS3Client(
                accessKeyId,
                secretAccessKey,
                Amazon.RegionEndpoint.GetBySystemName(region));
        }

        public async Task DeleteAsync(string pathOrContainerName, string fileName)
        {
            var deleteObjectRequest = new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = $"{pathOrContainerName}/{fileName}"
            };

            await _s3Client.DeleteObjectAsync(deleteObjectRequest);
        }

        public List<string> GetFiles(string pathOrContainerName)
        {
            var listRequest = new ListObjectsV2Request
            {
                BucketName = _bucketName,
                Prefix = pathOrContainerName
            };

            var response = _s3Client.ListObjectsV2Async(listRequest).Result;
            return response.S3Objects.Select(o => o.Key).ToList();
        }

        public bool CheckFileExists(string pathOrContainerName, string fileName)
        {
            var listRequest = new ListObjectsV2Request
            {
                BucketName = _bucketName,
                Prefix = $"{pathOrContainerName}/{fileName}"
            };

            var response = _s3Client.ListObjectsV2Async(listRequest).Result;
            return response.S3Objects.Any();
        }

        public async Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string pathOrContainerName, IFormFileCollection files)
        {
            var transferUtility = new TransferUtility(_s3Client);

            List<(string fileName, string pathOrContainerName)> datas = new();
            foreach (IFormFile file in files)
            {
                string fileNewName = await FileRenameAsync(pathOrContainerName, file.FileName, CheckFileExists);

                var uploadRequest = new TransferUtilityUploadRequest
                {
                    InputStream = file.OpenReadStream(),
                    Key = $"{pathOrContainerName}/{fileNewName}",
                    BucketName = _bucketName,
                    ContentType = file.ContentType
                };

                await transferUtility.UploadAsync(uploadRequest);

                datas.Add((fileNewName, pathOrContainerName));
            }

            return datas;
        }
    }

}
