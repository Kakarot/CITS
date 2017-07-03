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
            this.quiz = new QuizModel();
            this.MathProblems = quiz.GenerateMathProblems(2);
            this.CurrentProblemNumber = 0;

            this.ProblemContentPage = new ProblemPage();
            this.ProblemContentPage.SolutionSubmitted += CheckIfNewProblem;
            this.ProblemContentPage.HintRequested += HintRequested;

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

		public void HintRequested(object sender, EventArgs e)
		{
			//Enqueue a new problem when user requests a hint
			var newProblemQueue = quiz.GenerateMathProblems(1);
			MathProblems.Enqueue(newProblemQueue.Dequeue());
			quiz.NumberOfGeneratedProblems++;
		}

		public void CheckIfNewProblem(object sender, EventArgs e)
		{
            Boolean generateNewProblem = false;

            //Check if answer is correct
            if(MathProblems.Peek().IsSolutionCorrect(this.ProblemContentPage.Solution))
            {
                quiz.NumberOfCorrectAnswers++;
            }
            else
            {
                if(ProblemGenerator.rng.Next(0,2) == 1)
                {
                    generateNewProblem = true;
                }
            }

            //Remove currently answered problem
            MathProblems.Dequeue();

            if(generateNewProblem)
            {
                var newProblemQueue = quiz.GenerateMathProblems(1);
                MathProblems.Enqueue(newProblemQueue.Dequeue());
                quiz.NumberOfGeneratedProblems++;
            }
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
                this.ProblemContentPage.Hints = MathProblems.Peek().Hints;
				this.ProblemContentPage.UpdateProblemPage();

				if (Navigation.ModalStack.Count == 0)
				{
					await Navigation.PushModalAsync(this.ProblemContentPage);
				}
                this.ProblemContentPage.SolutionTextField.Focus();
				
            }
            else
            {
                //Dismiss problem page and show the results page 
                await Navigation.PopModalAsync();
                string pluralizeProblemString = String.Empty;
                if(quiz.NumberOfGeneratedProblems > 1)
                {
					pluralizeProblemString =
						Convert.ToString(quiz.NumberOfGeneratedProblems) +
                               " problems were generated during this quiz.";
                }
                else if(quiz.NumberOfGeneratedProblems == 1)
                {
                    pluralizeProblemString = "1 problem was generated for this quiz.";
                }
                else
                {
                    pluralizeProblemString = "No extra problems were generated for this quiz.";
                }
                this.ResultsContentPage.ResultsSummary =
                        $"You answered {quiz.NumberOfCorrectAnswers} out of {quiz.NumberOfProblems} problems correctly!";
                this.ResultsContentPage.NumberOfGeneratedProblems = pluralizeProblemString;
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
