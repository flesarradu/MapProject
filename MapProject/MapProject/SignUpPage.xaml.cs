using MapProject.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MapProject
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUpPage : ContentPage
    {
        private AzureService azureService;
        public SignUpPage(AzureService azureService)
        {
            InitializeComponent();
            registerButton.Clicked += RegisterButton_Clicked;
            this.azureService = azureService;
        }

        private async void RegisterButton_Clicked(object sender, EventArgs e)
        {
            string email = userEntry.Text;
            string password = passwordEntry.Text;
            string reenterPass = confirmPassEntry.Text;
            bool ready = true;
            if (checkIfEmailExists(email))
            {
                await DisplayAlert("Alert", "Email already exists, use another adress or login with your email.", "OK");
                ready = false;
            }
            if (password != reenterPass)
            {
                await DisplayAlert("Alert", "Your passwords does not match.", "OK");
                ready = false;
            }

            if (ready)
            { 
                //fa cont

                

            }
            
        }

        private bool checkIfEmailExists(string email)
        {
            return true;
        }
    }
}