using MapProject.DataModels;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MapProject
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            
        }

        protected async override void OnStart()
        {
            List<User> list = new List<User>();
            list.Add(new User { Id = "1", Username = "test", Password = "test" });
            
            var azureService = new AzureService();
            //await azureService.InsertUsers(list);
            var users = await azureService.GetUsers();
            MainPage = new MainPage();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
