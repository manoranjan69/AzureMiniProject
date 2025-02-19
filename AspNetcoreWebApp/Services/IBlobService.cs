﻿using AspNetcoreWebApp.Models;

namespace AspNetcoreWebApp.Services
{
    public interface IBlobService
    {

        Task<string> GetBlob(string name, string containerName);
        Task<List<string>> GetAllBlobs(string containerName);

        Task<bool> UploadBlob(string name, IFormFile file, string containerName, Blob blob);

        Task<bool> DeleteBlob(string name, string containerName);

        Task<List<Blob>> GetAllBlobwithUri(string containerName);
    }
}
