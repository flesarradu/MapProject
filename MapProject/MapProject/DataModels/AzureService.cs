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
        IMobileServiceSyncTable<Users> table;
        IMobileServiceTable<Users> userTable;

        public AzureService()
        {
            if (client?.SyncContext?.IsInitialized ?? false)
            {
                return;
            }
            var azureUrl = "https://mapprojecbackend.azurewebsites.net";

            client = new MobileServiceClient(azureUrl);
        }
        public async Task Initialize()
        {
            if(client?.SyncContext?.IsInitialized ?? false)
            {
                return;
            }
            var azureUrl = "https://mapprojecbackend.azurewebsites.net";

            client = new MobileServiceClient(azureUrl);

           

            //var path = "map.db";
            //path = path.combine(mobileserviceclient.defaultdatabasepath, path);

            //var store = new mobileservicesqlitestore(path);

            //store.definetable<users>();

            //await client.synccontext.initializeasync(store, new mobileservicesynchandler());

            //table = client.GetSyncTable<Users>();
            
            userTable = client.GetTable<Users>();
           
            //var results = await userTable.ReadAsync();

           
        }
        public async Task<Users> GetUser(string user)
        {
            await Initialize();
            var users = await client.GetTable<Users>().ReadAsync<Users>($"SELECT * FROM Users WHERE user=`{user}`");
            return users.FirstOrDefault(x=>x.User.Trim()==user);
        }

        public async Task <List<Users>> GetUsers()
        {
            await Initialize();
            //await SyncUsers();
            // return await table.ToListAsync();
            return await userTable.ToListAsync();
        }
        public async Task SyncUsers()
        {
            try
            {
                //await client.SyncContext.PushAsync();
                await table.PullAsync("allUsers", table.CreateQuery());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Debug.WriteLine("Unable to sync, using offline db.");
            }

        }
        public async Task InsertUsers(List<Users> users)
        {
            await Initialize();
            await Task.WhenAll(users.Select(x => userTable.InsertAsync(x)));
            //await SyncUsers();
        }

        public async Task InsertUser(Users user)
        {
            await Initialize();
            await userTable.InsertAsync(user);
        }

        public async Task<bool> CheckEmail(string email)
        {
            var users = await client.GetTable<Users>().ReadAsync<Users>($"SELECT * FROM Users WHERE user=`{email}`");
            var user = users.FirstOrDefault(x => x.User.Trim() == email);

            return user != null ? true : false;

        }
        
    }
}
