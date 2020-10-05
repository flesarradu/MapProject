using Android.Content;
using Java.Lang;
using MapProject.DataModels;
using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MapProject.NewFolder
{
    [BroadcastReceiver]
    public class BackgroundServiceReceiver : BroadcastReceiver
    {
        public static string ACTION_PROCESS_LOCATION = "XamarinGoogleMapsBackgroundLocation.UPDATE_LOCATION";
        public Users client;
        public AzureService azureService = new AzureService();
        public override void OnReceive(Context context, Intent intent)
        {
            if(intent != null)
            {
                string action = intent.Action;
                string username = intent.GetStringExtra("user");
                Task task = getClient(username); task.Wait();
                if (action.Equals(ACTION_PROCESS_LOCATION))
                {
                    try
                    {
                        Task updateTask = updateUserLocation();
                        updateTask.Wait();
                    }
                    catch (System.Exception)
                    {
                        Task killedAppUpdateTask = updateUserLocation();
                        killedAppUpdateTask.Wait();
                    }
                }
            }
        }

        private async Task updateUserLocation()
        {
            var locator = CrossGeolocator.Current;
            var position = await locator.GetPositionAsync();
            client.Latitude = position.Latitude;
            client.Longitude = position.Longitude;
            await azureService.UpdateUserAsync(client);
        }

        private async Task getClient(string user)
        {
            await azureService.Initialize();
            client = await azureService.GetUser(user);
            return;
        }
    }
}
