using AssetManagementTool_API.Services.IServices;
using AssetManagementTool_Common.Extensions;
using AssetManagementTool_Common.Models;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AssetManagementTool_API.Services
{
    public class BlobService : IBlobService
    {

        private readonly BlobServiceClient _blolbServiceClient;
        private readonly IConfiguration _configuration;


        public BlobService(BlobServiceClient blolbServiceClient, IConfiguration configuration)
        {
            _blolbServiceClient = blolbServiceClient;
            _configuration = configuration;
        }
        public async Task<BlobInfo> GetBlobAsync(string blobName, string containerName)
        {
            var containerClient = _blolbServiceClient.GetBlobContainerClient(containerName);
            var bobClient = containerClient.GetBlobClient(blobName);
            var blobDownloadInfo = await bobClient.DownloadAsync();


            return new BlobInfo(blobDownloadInfo.Value.Content, blobDownloadInfo.Value.ContentType);
        }
        public string GetBlobURL(string blobName, string containerName)
        {
            var containerClient = _blolbServiceClient.GetBlobContainerClient(containerName);
            var bobClient = containerClient.GetBlobClient(blobName);

            //To avoid hot linking we can create a token with expiration date and change the blob to private
            string uri = bobClient.Uri.AbsoluteUri;
            string sasToken = GenerateSASToken(blobName, containerName);
            return $"{uri}?{sasToken}";
        }

        public string GetBlobURL(string blobName, string containerName, int tokenLifeMinutes)
        {
            var containerClient = _blolbServiceClient.GetBlobContainerClient(containerName);
            var bobClient = containerClient.GetBlobClient(blobName);

            //To avoid hot linking we can create a token with expiration date and change the blob to private
            string uri = bobClient.Uri.AbsoluteUri;
            string sasToken = GenerateSASToken(blobName, containerName, tokenLifeMinutes);
            return $"{uri}?{sasToken}";
        }

        private string GenerateSASToken(string blobName, string containerName, int tokenLifeMinutes = 60)
        {
            
            var sharedKeyCredential = new StorageSharedKeyCredential(
                _configuration["BLOB_ACCOUNT_NAME"],
                _configuration["BLOB_ACCOUNT_KEY"]
                );

            var blobSasBuilder = new BlobSasBuilder
            {
                BlobContainerName = containerName,
                BlobName = blobName,
                StartsOn = DateTime.UtcNow,
                ExpiresOn = DateTime.UtcNow.AddMinutes(tokenLifeMinutes),
                Protocol = SasProtocol.HttpsAndHttp,
            };

            blobSasBuilder.SetPermissions(BlobAccountSasPermissions.Read);

            // Use the key to get the SAS token.
            string sasToken = blobSasBuilder.ToSasQueryParameters(sharedKeyCredential).ToString();
            return sasToken;
        }

        public async Task DeleteBlobAsync(string blobName, string containerName)
        {
            var containerClient = _blolbServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(blobName);
            await blobClient.DeleteIfExistsAsync();
        }

        public async Task<string> UploadContentBlobAsync(byte[] bytes, string fileName, string containerName)
        {
            try
            {
                var containerClient = _blolbServiceClient.GetBlobContainerClient(containerName);


                var blobClient = containerClient.GetBlobClient(fileName);
                var contentType = new Azure.Storage.Blobs.Models.BlobHttpHeaders { ContentType = fileName.GetContentType() };

                //Upload file
                await using MemoryStream stream = new MemoryStream(bytes);
                var response = await blobClient.UploadAsync(stream, contentType);

                return fileName;
            }
            catch(Exception ex)
            {
                return "";
            }


        }

        public async Task UploadFileBlobAsync(string filePath, string fileName, string containerName)
        {
            var containerClient = _blolbServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(fileName);

            var contentType = new Azure.Storage.Blobs.Models.BlobHttpHeaders { ContentType = filePath.GetContentType() };

            var res = await blobClient.UploadAsync(filePath, contentType);
        }


    }
}
