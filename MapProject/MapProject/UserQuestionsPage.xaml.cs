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
    public partial class UserQuestionsPage : ContentPage
    {
       
        private TaskCompletionSource<bool> questionsCompleted;
        private List<string> response = new List<string>(3);
        public UserQuestionsPage()
        {
            InitializeComponent();
            nationalityPicker.Items.Add("Roman");        
            questionsCompleted = new TaskCompletionSource<bool>();
        }
        public async Task<List<string>> Test()
        {
            await questionsCompleted.Task;
            return response;
        }

       

        private void okButton_Clicked(object sender, EventArgs e)
        {
            if (heightEntry.Text != "" && weightEntry.Text != "" && nationalityPicker.SelectedItem != null)
            {
                response.Add(heightEntry.Text);
                response.Add(weightEntry.Text);
                response.Add(nationalityPicker.SelectedItem.ToString());
                questionsCompleted.SetResult(true);
            }
            else
            {
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
                        {      
                            var task1 = DisplayAlert("Alert!", "You have not completed the right account information!", "Ok");
                            task1.Wait();
                            break; 
                        }
                    case Device.Android:
                        {
                            var task2 = DisplayAlert("Alert!", "You have not completed the right account information!", "Ok");
                            task2.Wait();
                            break; 
                        }

                }
            }
        }
    }
}