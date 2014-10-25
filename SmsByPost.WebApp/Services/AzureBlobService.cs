using System;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using SmsByPost.Models;

namespace SmsByPost.Services
{
    public class AzureBlobService
    {
        private readonly CloudBlobContainer _container;

        public AzureBlobService()
        {
            var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));

            var blobClient = storageAccount.CreateCloudBlobClient();
            
            _container = blobClient.GetContainerReference("letters");
            _container.CreateIfNotExists();
        }

        public void UploadToAzureBlobStore(Letter letter)
        {
            var blockBlob = _container.GetBlockBlobReference(letter.Id.ToString());

            var serializedLetter = JsonConvert.SerializeObject(letter);
            
            blockBlob.UploadText(serializedLetter);
        }

        public Letter GetFromBlobStore(Guid id)
        {
            var blockBlob = _container.GetBlockBlobReference(id.ToString());

            var serializedLetter = blockBlob.DownloadText();

            return JsonConvert.DeserializeObject<Letter>(serializedLetter);
        }
    }
}