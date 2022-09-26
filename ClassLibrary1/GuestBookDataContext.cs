using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using System.Linq;

namespace GuestBook_Data
{
    public class GuestBookDataContext : TableServiceContext
    {
        public IQueryable<GuestBookEntry> GuestBookEntry
        {
            get => CreateQuery<GuestBookEntry>("GuestBookEntry");
        }

        public GuestBookDataContext(string baseAddress, StorageCredentials credentials)
        : base(baseAddress, credentials)
        { }
    }
}
