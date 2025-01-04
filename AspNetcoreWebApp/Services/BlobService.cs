using AspNetcoreWebApp.Models;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Sas;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.CodeAnalysis;
using System.ComponentModel.DataAnnotations;

namespace AspNetcoreWebApp.Services
{
    public class BlobService : IBlobService
    {

        private readonly BlobServiceClient _blobclient;
        public BlobService(BlobServiceClient blobclient)
        {
            _blobclient = blobclient;
        }

        public async Task<bool> DeleteBlob(string name, string containerName)
        {
            BlobContainerClient blobContainerClient = _blobclient.GetBlobContainerClient(containerName);
            var blobclient = blobContainerClient.GetBlobClient(name);
            return await blobclient.DeleteIfExistsAsync();
        }

        public async Task<List<string>> GetAllBlobs(string containerName)
        {
            BlobContainerClient blobContainerClient = _blobclient.GetBlobContainerClient(containerName);
            var blob = blobContainerClient.GetBlobsAsync();
            var blobString = new List<string>();
            await foreach (var item in blob)
            {
                blobString.Add(item.Name);
            }
            return blobString;
        }

        public async Task<List<Blob>> GetAllBlobwithUri(string containerName)
        {
            BlobContainerClient blobcontainerclient = _blobclient.GetBlobContainerClient(containerName);
            var blobs = blobcontainerclient.GetBlobsAsync();
            var bloblist = new List<Blob>();
            await foreach (var item in blobs)
            {
                var blobclient = blobcontainerclient.GetBlobClient(item.Name);
                Blob blobIndividual = new()
                {
                    Uri = blobclient.Uri.AbsoluteUri
                };

                BlobProperties blobProperties = await blobclient.GetPropertiesAsync();

                //if (blobclient.CanGenerateSasUri)
                //{
                //    BlobSasBuilder sasBuilder = new()
                //    {
                //        BlobContainerName = blobclient.GetParentBlobContainerClient().Name,
                //        BlobName = blobclient.Name,
                //        Resource = "b",
                //        ExpiresOn = DateTime.UtcNow.AddHours(1)
                //    };
                //    sasBuilder.SetPermissions(BlobSasPermissions.Read);
                //    blobIndividual.Uri = blobclient.GenerateSasUri(sasBuilder).AbsoluteUri;
                //}
                if (blobProperties.Metadata.ContainsKey("Title"))
                {
                    blobIndividual.Title = blobProperties.Metadata["Title"];

                }

                if (blobProperties.Metadata.ContainsKey("Comment"))
                {
                    blobIndividual.Title = blobProperties.Metadata["Comment"];

                }
                bloblist.Add(blobIndividual);

            }
            return bloblist;

        }

        public async Task<string> GetBlob(string name, string containerName)
        {
            BlobContainerClient blobContainerClient = _blobclient.GetBlobContainerClient(containerName);
            var blobclient = blobContainerClient.GetBlobClient(name);
            return blobclient.Uri.AbsoluteUri;

        }

        public async Task<bool> UploadBlob(string name, IFormFile file, string containerName, Blob blob)
        {
            BlobContainerClient blobContainerClient = _blobclient.GetBlobContainerClient(containerName);
            var blobclient = blobContainerClient.GetBlobClient(name);
            var httpsHeader = new BlobHttpHeaders()
            {
                ContentType = file.ContentType
            };

            Dictionary<string, string> Metadata = new Dictionary<string, string>();
            Metadata.Add("Title", blob.Title);
            Metadata["Comment"] = blob.Comment;
            var result = await blobclient.UploadAsync(file.OpenReadStream(), httpsHeader, Metadata);
            Metadata.Remove("Title");
            await blobclient.SetMetadataAsync(Metadata);
            if (result != null)
            {
                return true;

            }
            return false;

        }
    }
}
