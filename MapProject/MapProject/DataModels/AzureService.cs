using MapProject.DataModels;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(AzureService))]
namespace MapProject.DataModels
{
    public class AzureService
    {
        MobileServiceClient client { get; set; }
        IMobileServiceSyncTable<User> table;

        public async Task Initialize()
        {
            if(client?.SyncContext?.IsInitialized ?? false)
            {
                return;
            }
            var azureUrl = "https://mapprojecbackend.azurewebsites.net";

            client = new MobileServiceClient(azureUrl);

            var path = "map.db";
            path = Path.Combine(MobileServiceClient.DefaultDatabasePath, path);

            var store = new MobileServiceSQLiteStore(path);

            store.DefineTable<User>();

            await client.SyncContext.InitializeAsync(store, new MobileServiceSyncHandler());

            table = client.GetSyncTable<User>();
        }

        public async Task <List<User>> GetUsers()
        {
            await Initialize();
            await SyncUsers();
            return await table.ToListAsync();
        }
        public async Task SyncUsers()
        {
            try
            {
                await client.SyncContext.PushAsync();
                await table.PullAsync("allUsers", table.CreateQuery());
            }
            catch
            {
                Debug.WriteLine("Unable to sync, using offline db.");
            }

        }
        public async Task InsertUsers(List<User> users)
        {
            await Initialize();
            await Task.WhenAll(users.Select(x => table.InsertAsync(x)));
            await SyncUsers();
        }
        
    }
}
