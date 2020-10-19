using Rg.Plugins.Popup.Extensions;
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
    public partial class FavouritesPlacesPopUp : Rg.Plugins.Popup.Pages.PopupPage
    {
        private IDictionary<string, object> properties = Application.Current.Properties;
        public FavouritesPlacesPopUp()
        {
            InitializeComponent();
            string fav_s = "";
            if (properties.ContainsKey("favourites-array"))
            {
                fav_s = (string)properties["favourites-array"];
            }
            else
            {
                fav_s = "1,1,1,1,1,1,1,1,1";
            }
            var fav = fav_s.Split(',').Select(Int32.Parse).ToList<int>();
            if (fav[0] == 1) checkBox1.IsChecked = true;
            if (fav[1] == 1) checkBox2.IsChecked = true;
            if (fav[2] == 1) checkBox3.IsChecked = true;
            if (fav[3] == 1) checkBox4.IsChecked = true;
            if (fav[4] == 1) checkBox5.IsChecked = true;
            if (fav[5] == 1) checkBox6.IsChecked = true;
            if (fav[6] == 1) checkBox7.IsChecked = true;
            if (fav[7] == 1) checkBox8.IsChecked = true;
            if (fav[8] == 1) checkBox9.IsChecked = true;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        // ### Methods for supporting animations in your popup page ###

        // Invoked before an animation appearing
        protected override void OnAppearingAnimationBegin()
        {
            base.OnAppearingAnimationBegin();
        }

        // Invoked after an animation appearing
        protected override void OnAppearingAnimationEnd()
        {
            base.OnAppearingAnimationEnd();
        }

        // Invoked before an animation disappearing
        protected override void OnDisappearingAnimationBegin()
        {
            base.OnDisappearingAnimationBegin();
        }

        // Invoked after an animation disappearing
        protected override void OnDisappearingAnimationEnd()
        {
            base.OnDisappearingAnimationEnd();
        }

        protected override Task OnAppearingAnimationBeginAsync()
        {
            return base.OnAppearingAnimationBeginAsync();
        }

        protected override Task OnAppearingAnimationEndAsync()
        {
            return base.OnAppearingAnimationEndAsync();
        }

        protected override Task OnDisappearingAnimationBeginAsync()
        {
            return base.OnDisappearingAnimationBeginAsync();
        }

        protected override Task OnDisappearingAnimationEndAsync()
        {
            return base.OnDisappearingAnimationEndAsync();
        }

        // ### Overrided methods which can prevent closing a popup page ###

        // Invoked when a hardware back button is pressed
        protected override bool OnBackButtonPressed()
        {
            // Return true if you don't want to close this popup page when a back button is pressed
            return base.OnBackButtonPressed();
        }

        // Invoked when background is clicked
        protected override bool OnBackgroundClicked()
        {
            // Return false if you don't want to close this popup page when a background of the popup page is clicked
            return base.OnBackgroundClicked();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            base.OnBackgroundClicked();
        }

        private void checkBox1_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {

        }

        private void checkBox2_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {

        }

        private void checkBox3_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {

        }

        private void checkBox4_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {

        }

        private void checkBox5_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {

        }

        private void checkBox6_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {

        }

        private void checkBox7_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {

        }

        private void checkBox8_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {

        }

        private void checkBox9_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {

        }

        private void answerButton_Clicked(object sender, EventArgs e)
        {
            int[] fav = new int[9];
            for (int i = 0; i < 9; i++) fav[i] = 0;
            if (checkBox1.IsChecked) fav[0] = 1;
            if (checkBox2.IsChecked) fav[1] = 1;
            if (checkBox3.IsChecked) fav[2] = 1;
            if (checkBox4.IsChecked) fav[3] = 1;
            if (checkBox5.IsChecked) fav[4] = 1;
            if (checkBox6.IsChecked) fav[5] = 1;
            if (checkBox7.IsChecked) fav[6] = 1;
            if (checkBox8.IsChecked) fav[7] = 1;
            if (checkBox9.IsChecked) fav[8] = 1;
            string X = string.Join(",",fav);
            properties["favourites-array"] = X;
            
        }
    }
}