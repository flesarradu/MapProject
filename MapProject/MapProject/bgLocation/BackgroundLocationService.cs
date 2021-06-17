using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Java.Util;
using MapProject.DataModels;
using Plugin.Geolocator;
using Xamarin.Forms;

namespace MapProject.Droid
{
    [Service(Label = "BackgroundLocationService")]
    public class BackgroundLocationService : Service
    {
        int counter = 0;
        bool isRunningTimer = true;
        public Users user;
        public override IBinder OnBind(Intent intent)
        {
            throw new NotImplementedException();
        }
        [return: GeneratedEnum]
        public void debug()
        {

        }
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            Log.Debug("SS", "Location Service Running");
            var task = GetUsersLocation(intent.GetStringExtra("user"));
            task.Wait();
            return StartCommandResult.NotSticky;
        }
        public override bool StopService(Intent name)
        {
            return base.StopService(name);
        }
        public override void OnDestroy()
        {
            StopSelf();
            counter = 0;
            isRunningTimer = false;
            base.OnDestroy();
        }
        private async Task GetUsersLocation(string username)
        {
            var locator = CrossGeolocator.Current;
            var position = await locator.GetPositionAsync();
            AzureService azureService = new AzureService();
            user = await azureService.GetUser(username);
            user.Latitude = position.Latitude;
            user.Longitude = position.Longitude;
            await azureService.Initialize();
            await azureService.UpdateUserAsync(user);
        }
    }
}