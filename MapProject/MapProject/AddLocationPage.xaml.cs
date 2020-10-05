using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MapProject.DataModels;

namespace MapProject
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddLocationPage : ContentPage
    {
        Places placeToAdd = new Places();
        AzureService azureService;
        public AddLocationPage(double lat, double lon, AzureService az)
        {
            InitializeComponent();
            placeToAdd.Latitude = lat;
            placeToAdd.Longitude = lon;
            azureService = az;
            List<string> places = new List<string>() { "restaurantapp", "restaurantnonapp", "hospitalapp", "hospitalnonapp", "barberapp", "barbernonapp", "dentistapp", "dentistnonapp" };
            locationTypePicker.ItemsSource = places;
        }

        private void okButton_Clicked(object sender, EventArgs e)
        {
            placeToAdd.LocationName = locationNameEntry.Text.Trim();
            placeToAdd.Type = locationTypePicker.SelectedItem.ToString();
            azureService.InsertPlace(placeToAdd);
            var task = DisplayAlert("INFO", "The place has been added", "OK");
            task.Wait();
            Navigation.PopAsync();
        }
    }
}