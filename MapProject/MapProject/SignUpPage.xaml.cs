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
            loginButton.Clicked += LoginButton_ClickedAsync;
            this.azureService = azureService;
        }

        private async void LoginButton_ClickedAsync(object sender, EventArgs e)
        {
            // App.Current.MainPage = new NavigationPage(new LoginPage(azureService));
            App.Current.MainPage = await Navigation.PopAsync();
        }

        private async void RegisterButton_Clicked(object sender, EventArgs e)
        {
            Users newUser = new Users();
            string email = userEntry.Text;
            string password = passwordEntry.Text;
            string reenterPass = confirmPassEntry.Text;
            bool ready = true;
            bool emailExists = await azureService.CheckEmail(email);
            if (emailExists)
            {
                await DisplayAlert("Problem", "Email already exists, use another adress or login with your email.", "OK");
                ready = false;
            }
            if (password != reenterPass)
            {
                await DisplayAlert("Problem", "Your passwords does not match.", "OK");
                ready = false;
            }

            
            if (ready)
            {
                //fa cont
                //newUser.Id = ;
                newUser.User = email;
                newUser.Password = password;
                await azureService.InsertUser(newUser);            
            }
            else
            {
                await DisplayAlert("Problem", "Reenter your data correctly","OK");
            }


            
        }

        
    }
}