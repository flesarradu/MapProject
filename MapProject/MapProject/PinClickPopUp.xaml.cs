using MapProject.DataModels;
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
    public partial class PinClickPopup : Rg.Plugins.Popup.Pages.PopupPage
    {
        public string Name;
        private Reviews review;
        private AzureService azureService;
        public PinClickPopup(string pinName)
        {
            InitializeComponent();
            Name = pinName;
            labelPlaceName.Text = Name;
            labelRating.Text = review.Rating.ToString();
            azureService = new AzureService();
        }
        public PinClickPopup(Reviews review1)
        {
            InitializeComponent();
            review = review1;
            labelPlaceName.Text = review.Location;
            labelRating.Text = review.Rating.ToString();
            azureService = new AzureService();
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
            OnBackButtonPressed();
        }

        private void ratingSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            ratingSlider.Value = Math.Round(ratingSlider.Value);
            yourRatingLabel.Text = ratingSlider.Value.ToString();
            if (ratingSlider.Value < 5)
            {
                ratingSlider.ThumbColor = Color.Red;
            }
            else
            {
                ratingSlider.ThumbColor = Color.FromRgb(0, 255, 0);
            }
        }

        private void rateButton_Clicked(object sender, EventArgs e)
        {
            review.Rating = (int)Math.Round(ratingSlider.Value);
            azureService.InsertReview(review);
            Task t = DisplayAlert("Review",$"Your review on {review.Location} has been received","Ok!");
            t.Wait();
        }
    }
}