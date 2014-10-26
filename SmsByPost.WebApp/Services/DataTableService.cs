using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using SMSByPost.MessageDispatcher.WorkerRole.Models;

namespace SMSByPost.MessageDispatcher.WorkerRole.Services
{
    class DataTableService
    {
        private static CloudTable _table;

        public DataTableService()
        {
            var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));

            var tableClient = storageAccount.CreateCloudTableClient();
            _table = tableClient.GetTableReference("clockwork");
        }

        public void StoreId(string letterId, string clockworkId)
        {
            _table.CreateIfNotExists();

            var letterEntity = new LetterEntity(letterId, clockworkId);

            var insertOperation = TableOperation.Insert(letterEntity);
            _table.Execute(insertOperation);
        }
    }
}
