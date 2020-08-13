using MapProject.DataModels;
using System;
using System.Collections;
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

        private IDictionary<string,object> properties = Application.Current.Properties;

        private bool rememberMe = false;
        private LoginInformation login;
        public LoginPage(AzureService azureService)
        {
            InitializeComponent();
            loginButton.Clicked += LoginButton_ClickedAsync;
            registerButton.Clicked += RegisterButton_Clicked;
            buttonClick = new TaskCompletionSource<bool>();
            this.azureService = azureService;
            login = new LoginInformation();
            //Navigation.PushAsync(new SignUpPage(azureService));
            string savedUsername = "";
            string savedPassword = "";

            if (properties.ContainsKey("username") && properties.ContainsKey("password"))
            {
                savedUsername = (string)properties["username"];
                savedPassword = (string)properties["password"];
                rememberMeCheckBox.IsChecked = true;
            }

            userEntry.Text = savedUsername;
            passwordEntry.Text = savedPassword;
            
        }

        private async void RegisterButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignUpPage(azureService));
        }

        public async Task<LoginInformation> Login()
        {
            await buttonClick.Task;
            login.LoggedIn = loggedIn;
            return login;   
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
                    if (rememberMe)
                    {
                        if(properties.ContainsKey("username") && properties.ContainsKey("password"))
                        {
                            properties["username"] = user.User.Trim();
                            properties["password"] = user.Password.Trim();
                        }
                        else
                        {
                            properties.Add("username", user.User.Trim());
                            properties.Add("password", user.Password.Trim());
                        }
                    }
                    else
                    {
                        properties.Remove("username");
                        properties.Remove("password");
                    }
                    login.User = user;
                    buttonClick.SetResult(true);
                }
                else
                {
                    loggedIn = false;
                }
            }
            
        }

        private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            rememberMe = rememberMeCheckBox.IsChecked;
        }
    }
}
