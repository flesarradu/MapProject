using MapProject.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MapProject
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class LoginPage : ContentPage
    {
        private TaskCompletionSource<bool> buttonClick;

        private AzureService azureService;

        private bool loggedIn = false;
        public LoginPage(AzureService azureService)
        {
            InitializeComponent();
            loginButton.Clicked += LoginButton_ClickedAsync;
            registerButton.Clicked += RegisterButton_Clicked;
            buttonClick = new TaskCompletionSource<bool>();
            this.azureService = azureService;
            //Navigation.PushAsync(new SignUpPage(azureService));
                        
        }

        private async void RegisterButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignUpPage(azureService));
        }

        public async Task<bool> Login()
        {
            await buttonClick.Task;
            return loggedIn;   
        }

        private async Task<Users> GetUser(string username)
        {
            return await azureService.GetUser(username);
        }

        private async void LoginButton_ClickedAsync(object sender, EventArgs e)
        {
            string username = userEntry.Text;   
            string password = passwordEntry.Text;
            var user =  await GetUser(username);

            if (user!=null)
            {
                if (user.Password.Trim() == password)
                {
                    loggedIn = true;
                    buttonClick.SetResult(true);
                }
                else
                {
                    loggedIn = false;
                }
            }
            
        }       
    }
}
