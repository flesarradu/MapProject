using MapProject.DataModels;
using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;

namespace MapProject
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        

        public MainPage(List<Cases> cases)
        {
            InitializeComponent();
            var task = setGeoPosition(map);
            Cases = cases;
        }

        public List<Cases> Cases { get; }

        private async Task setGeoPosition(Map map)
        {
            var locator = CrossGeolocator.Current;
            var position = await locator.GetPositionAsync();
            map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude), Distance.FromKilometers(10)));
            map.UiSettings.ZoomControlsEnabled = false;
            Circle testCircle = new Circle { Center = new Position(position.Latitude, position.Longitude), Radius = Distance.FromMeters(30), FillColor = Color.AliceBlue, Tag = "TEST" };
            map.Circles.Add(testCircle);
            return;
        }
    }
}