using Microsoft.WindowsAzure.Storage.Table;

namespace SMSByPost.MessageDispatcher.WorkerRole.Models
{
    internal class LetterEntity : TableEntity
    {
        public LetterEntity(string letterId, string clockworkId)
        {
            PartitionKey = letterId;
            RowKey = clockworkId;
        }
    }
}