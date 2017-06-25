using System;
using System.Collections.Generic;
using CITS.Models;
using Xamarin.Forms;

namespace CITS
{
    public partial class OrderOfOperationsPage : ContentPage
    {      
        private QuizModel quiz
        {
            get;
            set;
        }

        private Queue<MathProblemModel> MathProblems
        {
            get;
            set;
        }

        private ProblemPage ProblemContentPage
        {
            get;
            set;
        }

        private ResultsPage ResultsContentPage
        {
            get;
            set;
        }
        public OrderOfOperationsPage()
        {
            InitializeComponent();
            this.quiz = new QuizModel();
        }

        private int CurrentProblemNumber
        {
            get;
            set;
        }


        void Handle_Clicked_Start_Quiz(object sender, System.EventArgs e)
        {
            //await DisplayAlert("Sorry!", "The quiz feature has not " +
            // "been implemented yet, please check if new build " +
            // "has it!",
            //"OK");

            //TODO: Instantiate Quiz Model
            //TODO: Generate List<MathProblemModel>
            //TODO: Ensure last page is ResultsPage, Pass QuizModel
            this.MathProblems = quiz.GenerateMathProblems();
            this.CurrentProblemNumber = 0;

            this.ProblemContentPage = new ProblemPage();
            this.ProblemContentPage.SolutionSubmitted += CheckIfNewProblem;

            this.ResultsContentPage = new ResultsPage();

            //Subscribe to page's close event
            this.ResultsContentPage.ResultsScreenDismissed += QuizCompleted;
            UpdateProblemPageWithProblem();

        //   await Navigation.PushModalAsync(this.ResultsContentPage);		
        }

		async public void QuizCompleted(object sender, EventArgs e)
		{
		//	DisplayAlert("Congrats!", "Great job finishing Quiz!", "OK");
            await Navigation.PopModalAsync();
		}

		public void CheckIfNewProblem(object sender, EventArgs e)
		{
            //Check if answer is correct
            if(MathProblems.Peek().IsSolutionCorrect(this.ProblemContentPage.Solution))
            {
                quiz.NumberOfCorrectAnswers++;
            }
            MathProblems.Dequeue();
            //Dequeue
            UpdateProblemPageWithProblem();

		}

        async private void UpdateProblemPageWithProblem()
        {
            if(MathProblems.Count !=0)
            {
                //Update labels or new problem
                CurrentProblemNumber++;
              
                //if (Navigation.ModalStack.Count > 0)
                //{
                //    await Navigation.PopModalAsync();
                //}
				this.ProblemContentPage.ProblemNumber = CurrentProblemNumber.ToString();
				this.ProblemContentPage.Problem = MathProblems.Peek().Problem;
				this.ProblemContentPage.UpdateProblemPage();

				if (Navigation.ModalStack.Count == 0)
				{
					await Navigation.PushModalAsync(this.ProblemContentPage);
				}
				
            }
            else
            {
                //Dismiss problem page and show the results page 
                await Navigation.PopModalAsync();
                this.ResultsContentPage.ResultsSummary =
                        $"You answered {quiz.NumberOfCorrectAnswers} out of {quiz.NumberOfProblems} problems correctly!";
                this.ResultsContentPage.UpdatePage();

                //Clear this text so that when the modal view is switching
                //the user does not breifly see previous test score
				this.PreviousSessionLabel.Text = String.Empty;
				this.RecommendationLabel.Text = String.Empty;

                await Navigation.PushModalAsync(this.ResultsContentPage);

                if(quiz.NumberOfCorrectAnswers == quiz.NumberOfProblems)
                {
                    this.PreviousSessionLabel.TextColor = Color.Blue;   
                }
                else
                {
                    this.PreviousSessionLabel.TextColor = Color.Red;
				}
                this.PreviousSessionLabel.Text =
                    $"Last Session Results: {quiz.NumberOfCorrectAnswers} correct out of {quiz.NumberOfProblems}";
                this.RecommendationLabel.Text = quiz.GetRecommendation();


			}
        }
    }
}
