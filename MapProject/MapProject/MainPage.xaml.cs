using Android;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Locations;
using Android.Widget;
using MapProject.DataModels;
using MapProject.Droid;
using MapProject.NewFolder;
using Newtonsoft.Json;
using Plugin.Geolocator;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
//using Windows.UI.Popups;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Markup;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.Xaml;

namespace MapProject
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        private Users user;
        private AzureService azureService;
        private BitmapDescriptor manImage = BitmapDescriptorFactory.FromBundle("man.png");
        private BitmapDescriptor blueManImage = BitmapDescriptorFactory.FromBundle("blueman.png");
        private BitmapDescriptor yellowManImage = BitmapDescriptorFactory.FromBundle("yellowman.png");
        private BitmapDescriptor redManImage = BitmapDescriptorFactory.FromBundle("redman.png");
        private BitmapDescriptor greenManImage = BitmapDescriptorFactory.FromBundle("greenman.png");
        private BitmapDescriptor restaurantCApp = BitmapDescriptorFactory.FromBundle("restaurant_covid_app.png");
        private BitmapDescriptor restaurantCNonApp = BitmapDescriptorFactory.FromBundle("restaurant_covid_nonapp.png");
        private BitmapDescriptor hospitalCApp = BitmapDescriptorFactory.FromBundle("hospitalapp.png");
        private BitmapDescriptor hospitalCNonApp = BitmapDescriptorFactory.FromBundle("hospitalnonapp.png");
        private BitmapDescriptor dentistCApp = BitmapDescriptorFactory.FromBundle("dentistapp.png");
        private BitmapDescriptor dentistCNonApp = BitmapDescriptorFactory.FromBundle("dentistnonapp.png");
        private BitmapDescriptor barberCApp = BitmapDescriptorFactory.FromBundle("barberapp.png");
        private BitmapDescriptor barberCNonApp = BitmapDescriptorFactory.FromBundle("barbernonapp.png");
        public static string ACTION_PROCESS_LOCATION = "XamarinGoogleMapsBackgroundLocation.UPDATE_LOCATION";
        private IDictionary<string, object> properties = Application.Current.Properties;
        List<int> fav;
        [Obsolete]
        public MainPage(Users user, AzureService azure)
        {
            InitializeComponent();
            azureService = azure;
            var task = setGeoPosition(map);
            this.user = user;
            
            var task3 = checkIfHasAnswered_INFO();
            var task2 = checkIfHasAnswered_RISK();
            LoadFavList(); 
            LoadCases();
            


            if (user.RiskLevel > 0)
            {
                switch (Device.RuntimePlatform)
                {
                    case Device.Android:
                        {
                            var intent = new Intent(Android.App.Application.Context, typeof(BackgroundServiceReceiver));
                            intent.PutExtra("user", user.User);
                            intent.SetAction(ACTION_PROCESS_LOCATION);
                            Android.App.Application.Context.StartService(intent);
                            break;
                        }
                }
            }
            //map.CameraMoveStarted += Map_CameraMoveStarted;
            map.CameraChanged += Map_CameraChanged;
            map.PinClicked += Map_PinClicked;
            map.MapClicked += Map_MapClicked;
            
        }

        private async void Map_MapClicked(object sender, MapClickedEventArgs e)
        {
            var lat = e.Point.Latitude;
            var lon = e.Point.Longitude;
            string s = $"https://maps.googleapis.com/maps/api/place/nearbysearch/json?location={lat},{lon}&radius=10&types=point_of_interest&key=AIzaSyBLRYPf8FWJws_x7jVqmdlP4m9an_qJMz8";
            using (var client = new HttpClient())
            {
                var response = await client.GetStringAsync(s);
                var result = JsonConvert.DeserializeObject<PlacesApiQueryResponse>(response);
                if (result.results.Count > 0)
                {
                    PinClickPopup popUp = new PinClickPopup(result.results[0].name);
                    await Navigation.PushPopupAsync(popUp, true);
                }
           
            }
        }

        private async void Map_PinClicked(object sender, PinClickedEventArgs e)
        {
            //e.Handled = true;
            //var uri = new Uri("http://maps.google.com/maps?daddr=" + e.Pin.Position.Latitude + "," + e.Pin.Position.Longitude);
            //Task t = Launcher.OpenAsync(uri);
            //t.Wait();
        }

        private void LoadFavList()
        {
            string fav_s = "";
            if (properties.ContainsKey("favourites-array"))
            {
                fav_s = (string)properties["favourites-array"];
            }
            else
            {
                fav_s = "1,1,1,1,1,1,1,1,1";
            }
            fav = fav_s.Split(',').Select(Int32.Parse).ToList<int>();
        }

        private async Task checkIfHasAnswered_INFO()
        {
            if (user.Height == -1 || user.Weight == -1 || user.Nationality.Equals(null))
            {
                bool response = false;
                switch (Device.RuntimePlatform)
                {
                    case Device.UWP:
                        {
                            //var messageDialog = new MessageDialog("Alert!", "You have not completed the health test! Do you want to start the test?");
                            //messageDialog.Commands.Add(new UICommand("Yes"));
                            //messageDialog.Commands.Add(new UICommand("No"));
                            //messageDialog.DefaultCommandIndex = 0;
                            //messageDialog.CancelCommandIndex = 1;
                            //await messageDialog.ShowAsync();
                            break;
                        }
                    case Device.iOS:
                        response = await DisplayAlert("Alert!", "You have not completed the account information! Do you want to complete it now?", "Yes", "No"); break;
                    case Device.Android:
                        response = await DisplayAlert("Alert!", "You have not completed the account information! Do you want to complete it now?", "Yes", "No"); break;
                }
                if (response)
                {
                    UserQuestionsPage userQuestionsPage = new UserQuestionsPage();
                    await Navigation.PushAsync(userQuestionsPage);
                    List<string> result = await userQuestionsPage.Test();
                    await Navigation.PopAsync();
                    user.Height = int.Parse(result[0]);
                    user.Weight = int.Parse(result[1]);
                    user.Nationality = result[2];
                    await azureService.UpdateUserAsync(user);
                }
            }
        }

        private void Map_CameraChanged(object sender, CameraChangedEventArgs e)
        {
            if(map.CameraPosition.Zoom < 10)
            {
                map.Pins.Clear();
            }
            else
            {
                LoadFavList();
                LoadPlaces();
                LoadCases();
            }

        }

        private async void LoadPlaces()
        {
            var places = await azureService.placesTable.ToListAsync();
            foreach (var p in places)
            {
                if (fav[getPlaceNumberFromType(p.Type) - 1] == 1)
                {
                    BitmapDescriptor pinIcon = restaurantCApp;
                    switch (p.Type)
                    {
                        case "restaurantapp": { pinIcon = restaurantCApp; break; }
                        case "restaurantnonapp": { pinIcon = restaurantCNonApp; break; }
                        case "hospitalapp": { pinIcon = hospitalCApp; break; }
                        case "hospitalnonapp": { pinIcon = hospitalCNonApp; break; }
                        case "dentistapp": { pinIcon = dentistCApp; break; }
                        case "dentistnonapp": { pinIcon = dentistCNonApp; break; }
                        case "barberapp": { pinIcon = barberCApp; break; }
                        case "barbernonapp": { pinIcon = barberCNonApp; break; }
                    }
                    Pin pin = new Pin
                    {
                        Label = p.LocationName,
                        Position = new Position(p.Latitude, p.Longitude),
                        Icon = pinIcon,
                        IsVisible = true
                    };
                    map.Pins.Add(pin);
                }
            }
        }
        private int getPlaceNumberFromType(string type)
        {
            switch (type)
            {
                case "restaurantapp": { return 8 ; }
                case "restaurantnonapp": { return 9; }
                case "hospitalapp": { return 2;  }
                case "hospitalnonapp": { return 3;  }
                case "dentistapp": { return 4;  }
                case "dentistnonapp": { return 5; }
                case "barberapp": { return 6;  }
                case "barbernonapp": { return 7; }
            }
            return 0;
        }
        private async void LoadCases()
        {
            if (fav[0] == 1)
            {
                var users = await azureService.userTable.ToListAsync();
                users.Remove(users.Where(x => x.User == user.User).FirstOrDefault());
                foreach (var u in users)
                {

                    if (u.RiskLevel > 0 && map.CameraPosition.Zoom > 10)
                    {
                        Pin pin = new Pin
                        {
                            Label = "Case",
                            Position = new Position(u.Latitude, u.Longitude),
                            Icon = (u.RiskLevel >= 40) ? greenManImage : (u.RiskLevel >= 30) ? blueManImage : (u.RiskLevel >= 20) ? redManImage : (u.RiskLevel >= 10) ? yellowManImage : manImage,
                            IsVisible = true
                        };

                        map.Pins.Add(pin);
                    }
                }
            }
        }

        private async Task checkIfHasAnswered_RISK()
        {
            if(user.RiskLevel == -1)
            {
                bool response = false;
                switch (Device.RuntimePlatform)
                {
                    case Device.UWP:
                        {
                            //var messageDialog = new MessageDialog("Alert!", "You have not completed the health test! Do you want to start the test?");
                            //messageDialog.Commands.Add(new UICommand("Yes"));
                            //messageDialog.Commands.Add(new UICommand("No"));
                            //messageDialog.DefaultCommandIndex = 0;
                            //messageDialog.CancelCommandIndex = 1;
                            //await messageDialog.ShowAsync();
                            break;
                        }
                    case Device.iOS:
                        response = await DisplayAlert("Alert!", "You have not completed the health test! Do you want to start the test?", "Yes", "No"); break;
                    case Device.Android:
                        response = await DisplayAlert("Alert!", "You have not completed the health test! Do you want to start the test?", "Yes", "No"); break;
                }
                if (response)
                {
                    QuestionPage questionPage = new QuestionPage();
                    await Navigation.PushAsync(questionPage);
                    int risk = await questionPage.Test();
                    user.RiskLevel = risk;
                    await azureService.UpdateUserAsync(user);
                    await Navigation.PopAsync();
                }    
            }
        }

        public List<Cases> Cases { get; }

        private async Task setGeoPosition(Xamarin.Forms.GoogleMaps.Map map)
        {
            var locator = CrossGeolocator.Current;
            var position = await locator.GetPositionAsync();
            map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude), Distance.FromKilometers(10)));
            map.UiSettings.ZoomControlsEnabled = false;
            if (user.RiskLevel > 0)
            {
                user.Latitude = position.Latitude;
                user.Longitude = position.Longitude;
                await azureService.UpdateUserAsync(user);
            }
            return;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            //AddLocationPage locationPage = new AddLocationPage(user.Latitude, user.Longitude, azureService);
            //await Navigation.PushAsync(locationPage);
            //FavouritesPlacesPopUp popUp = new FavouritesPlacesPopUp();
        }
        //GOOGLE PLACES API CLASS
        public class Location
        {
            public double lat { get; set; }
            public double lng { get; set; }
        }

        public class Geometry
        {
            public Location location { get; set; }
        }

        public class OpeningHours
        {
            public bool open_now { get; set; }
            public List<object> weekday_text { get; set; }
        }

        public class Photo
        {
            public int height { get; set; }
            public List<string> html_attributions { get; set; }
            public string photo_reference { get; set; }
            public int width { get; set; }
        }

        public class Result
        {
            public Geometry geometry { get; set; }
            public string icon { get; set; }
            public string id { get; set; }
            public string name { get; set; }
            public OpeningHours opening_hours { get; set; }
            public List<Photo> photos { get; set; }
            public string place_id { get; set; }
            public double rating { get; set; }
            public string reference { get; set; }
            public string scope { get; set; }
            public List<string> types { get; set; }
            public string vicinity { get; set; }
        }

        public class PlacesApiQueryResponse
        {
            public List<object> html_attributions { get; set; }
            public List<Result> results { get; set; }
            public string status { get; set; }
        }
    }
}