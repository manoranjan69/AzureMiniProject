using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace AspNetcoreWebApp.Services
{
    public class ContainerService : IContainerService
    {
        private BlobServiceClient _blobClient;
        public ContainerService(BlobServiceClient blobClient)
        {
            _blobClient = blobClient;
        }
        public async Task CreateContainer(string containerName)
        {
            BlobContainerClient blobcontainerclient = _blobClient.GetBlobContainerClient(containerName);
            await blobcontainerclient.CreateIfNotExistsAsync(PublicAccessType.BlobContainer);


        }

        public async Task DeleteContainer(string containerName)
        {
            BlobContainerClient blobcontainerclient = _blobClient.GetBlobContainerClient(containerName);
            await blobcontainerclient.DeleteIfExistsAsync();
        }

        public async Task<List<string>> GetAllContainer()
        {
            List<string> ContainerName = new();
            await foreach (BlobContainerItem blobcontaineritem in _blobClient.GetBlobContainersAsync())
            {
                ContainerName.Add(blobcontaineritem.Name);
            }
            return ContainerName;
        }

        public async Task<List<string>> GetAllContainerAndBlobs()
        {
            List<string> containersAndBlobNames = new();
            containersAndBlobNames.Add("AccountName :" + _blobClient.AccountName);
            containersAndBlobNames.Add("---------------------------------------------");
            await foreach (BlobContainerItem blobcontainerItem in _blobClient.GetBlobContainersAsync())
            {
                containersAndBlobNames.Add("--" + blobcontainerItem.Name);
                BlobContainerClient _blobContainer = _blobClient.GetBlobContainerClient(blobcontainerItem.Name);


                await foreach (BlobItem blobitem in _blobContainer.GetBlobsAsync())
                {

                    var _blobclient = _blobContainer.GetBlobClient(blobitem.Name);
                    BlobProperties _blobproperties = await _blobclient.GetPropertiesAsync();

                    string blobToAdd = blobitem.Name;
                    if (_blobproperties.Metadata.ContainsKey("Title"))
                    {
                        blobToAdd += "(" + _blobproperties.Metadata["Title"] + ")";
                    }
                    containersAndBlobNames.Add("--" + blobToAdd);

                    containersAndBlobNames.Add("--------------------------------------------------------------------------------------------------");

                    containersAndBlobNames.Add("------------------------------------------------------------------------" + blobitem.Name);
                }

                containersAndBlobNames.Add("------------------------------------------------------------------------");
            }
            return containersAndBlobNames;
        }
    }
}
