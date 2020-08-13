using MapProject.DataModels;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MapProject
{
    public partial class App : Application
    {
        public AzureService azureService;
        public List<Cases> cases;
        public App()
        {
            InitializeComponent();
            azureService = new AzureService();
        }

        protected async override void OnStart()
        {
            
            //SignUpPage signUp = new SignUpPage(azureService);
            LoginPage loginPage = new LoginPage(azureService);
            MainPage = new NavigationPage(loginPage);
            //cases = await azureService.GetCases();   
            var logged = await loginPage.Login();
            if (logged.LoggedIn)
            {
               MainPage = new NavigationPage(new MainPage(logged.User, azureService));
            }
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

    }
}
