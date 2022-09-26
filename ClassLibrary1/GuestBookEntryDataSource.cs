using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GuestBook_Data
{
    public class GuestBookEntryDataSource
    {
        private static CloudStorageAccount storageAccount;
        private GuestBookDataContext context;

        static GuestBookEntryDataSource()
        {
            storageAccount = CloudStorageAccount.FromConfigurationSetting("DataConnectionString");
            CloudTableClient.CreateTablesFromModel(typeof(GuestBookDataContext), storageAccount.TableEndpoint.AbsoluteUri, storageAccount.Credentials);
        }

        public GuestBookEntryDataSource()
        {
            context = new GuestBookDataContext(storageAccount.TableEndpoint.AbsoluteUri, storageAccount.Credentials);
            context.RetryPolicy = RetryPolicies.Retry(3, TimeSpan.FromSeconds(1));
        }

        public IEnumerable<GuestBookEntry> Select()
        {
            var results = from g in context.GuestBookEntry
                          select g;

            return results;
        }

        public void UpdateImageThumbnail(string partitionKey, string rowKey, string thumbUrl)
        {
            var results = from g in context.GuestBookEntry
                          where g.PartitionKey == partitionKey && g.RowKey == rowKey
                          select g;
            var entry = results.FirstOrDefault();
            entry.ThumbnailUrl = thumbUrl;
            context.UpdateObject(entry);
            context.SaveChanges();
        }

        public void AddGuestBookEntry(GuestBookEntry guestBook)
        {
            context.AddObject(nameof(GuestBookEntry), guestBook);
            context.SaveChanges();
        }
    }
}