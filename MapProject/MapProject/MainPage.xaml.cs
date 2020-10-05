using Android;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Locations;
using Android.Widget;
using MapProject.DataModels;
using MapProject.Droid;
using MapProject.NewFolder;
using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        [Obsolete]
        public MainPage(Users user, AzureService azure)
        {
            InitializeComponent();
            azureService = azure;
            var task = setGeoPosition(map);
            this.user = user;
            
            var task3 = checkIfHasAnswered_INFO();
            var task2 = checkIfHasAnswered_RISK();
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
                LoadPlaces();
                LoadCases();
            }

        }

        private async void LoadPlaces()
        {
            var places = await azureService.placesTable.ToListAsync();
            foreach(var p in places)
            {
                BitmapDescriptor pinIcon = restaurantCApp;
                switch(p.Type)
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

        private async void LoadCases()
        {
            var users = await azureService.userTable.ToListAsync();
            users.Remove(users.Where(x => x.User == user.User).FirstOrDefault());
            foreach(var u in users)
            {

                if (u.RiskLevel > 0 && map.CameraPosition.Zoom > 10)
                {
                    Pin pin = new Pin
                    {
                        Label = "Case",
                        Position = new Position(u.Latitude, u.Longitude),
                        Icon = (u.RiskLevel>=40)?greenManImage:(u.RiskLevel>=30)?blueManImage:(u.RiskLevel>=20)?redManImage:(u.RiskLevel>=10)?yellowManImage:manImage,
                        IsVisible = true
                    };
                                      
                     map.Pins.Add(pin);
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

        private async Task setGeoPosition(Map map)
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
            FavouritesPlacesPopUp popUp = new FavouritesPlacesPopUp();
            await Navigation.PushAsync(popUp);
        }
    }
}