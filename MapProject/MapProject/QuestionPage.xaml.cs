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
    public partial class QuestionPage : ContentPage
    {
        List<Question> questions = new List<Question>();
        private TaskCompletionSource<bool> questionsCompleted;
        int personRisk = 0;
        int currentQuestionId = 0;
        public QuestionPage()
        {
            InitializeComponent();
            InitializeQuestions();
            questionsCompleted = new TaskCompletionSource<bool>();
        }

        public async Task<int> Test()
        {
            await questionsCompleted.Task;
            return personRisk; //TO DO
        }

        private void InitializeQuestions()
        {
            questions.Add(new Question { QuestionText = "Do you have a fever?", Answer1 = "37-38", Answer2 = "38-39", Answer3 = "39+", Answer4 = "No", Answer1Risk = 1, Answer2Risk = 5, Answer3Risk = 20, Answer4Risk = 0 });
            questions.Add(new Question { QuestionText = "Do you have chills?", Answer1 = "Yes", Answer2 = "No", Answer1Risk = 20, Answer2Risk = 0 });
            loadQuestion(currentQuestionId);
        }

        private void loadQuestion(int v)
        {
            resetCheckBoxes();

            questionLabel.Text = questions[v].QuestionText;
            checkLabel1.Text = questions[v].Answer1;
            checkLabel2.Text = questions[v].Answer2;

            if (questions[v].Answer3 != "" && (questions[v].Answer3 != null))
            {
                checkLabel3.Text = questions[v].Answer3;
                checkBox3.IsVisible = true;
                checkLabel3.IsVisible = true;
            }
            else
            {
                checkBox3.IsVisible = false;
                checkLabel3.IsVisible = false;
            }

            if ((questions[v].Answer4 != "" ) && (questions[v].Answer4 != null))
            {
                checkLabel4.Text = questions[v].Answer4;
                checkBox4.IsVisible = true;
                checkLabel4.IsVisible = true;
            }
            else
            {
                checkBox4.IsVisible = false;
                checkLabel4.IsVisible = false;
            }

        }

        private void answerButton_Clicked(object sender, EventArgs e)
        {
            if (checkBox1.IsChecked)
            {
                personRisk += questions[currentQuestionId].Answer1Risk;
            }
            if (checkBox2.IsChecked)
            {
                personRisk += questions[currentQuestionId].Answer2Risk;
            }
            if (checkBox3.IsChecked)
            {
                personRisk += questions[currentQuestionId].Answer3Risk;
            }
            if (checkBox4.IsChecked)
            {
                personRisk += questions[currentQuestionId].Answer4Risk;
            }
            if (++currentQuestionId < questions.Count)
            {
                loadQuestion(currentQuestionId);
            }
            else
            {
                questionsCompleted.SetResult(true);
            }
        }
        //CheckBoxes Management
        private void resetCheckBoxes()
        {
            checkBox4.IsChecked = false;
            checkBox3.IsChecked = false;
            checkBox2.IsChecked = false;
            checkBox1.IsChecked = false;
        }

        private void checkBox1_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (checkBox1.IsChecked)
            {
                if (checkBox4.IsChecked) checkBox4.IsChecked = false;
                if (checkBox3.IsChecked) checkBox3.IsChecked = false;
                if (checkBox2.IsChecked) checkBox2.IsChecked = false;
            }
            else
            {
                checkBox1.IsChecked = false;
            }
            
        }

        private void checkBox2_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (checkBox2.IsChecked)
            {
                if (checkBox4.IsChecked) checkBox4.IsChecked = false;
                if (checkBox3.IsChecked) checkBox3.IsChecked = false;
                if (checkBox1.IsChecked) checkBox1.IsChecked = false;
            }
            else
            {
                checkBox2.IsChecked = false;
            }
        }

        private void checkBox3_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (checkBox3.IsChecked)
            {
                if (checkBox4.IsChecked) checkBox4.IsChecked = false;
                if (checkBox1.IsChecked) checkBox1.IsChecked = false;
                if (checkBox2.IsChecked) checkBox2.IsChecked = false;
            }
            else
            {
                checkBox3.IsChecked = false;
            }
        }

        private void checkBox4_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (checkBox4.IsChecked)
            {
                if (checkBox1.IsChecked) checkBox1.IsChecked = false;
                if (checkBox3.IsChecked) checkBox3.IsChecked = false;
                if (checkBox2.IsChecked) checkBox2.IsChecked = false;
            }
            else
            {
                checkBox4.IsChecked = false;
            }
        }
    }
}