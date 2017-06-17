using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace CITS
{
    public partial class OrderOfOperationsPage : ContentPage
    {      
        public OrderOfOperationsPage()
        {
            InitializeComponent();
        }

        async void Handle_Clicked_Start_Quiz(object sender, System.EventArgs e)
        {
           await DisplayAlert("Sorry!", "The quiz feature has not " +
                         "been implemented yet, please check if new build " +
                         "has it!",
                        "OK");

            //TODO: Instantiate Quiz Model
            //TODO: Generate List<MathProblemModel>
            //TODO: Ensure last page is ResultsPage, Pass QuizModel

            var resultsPage = new ResultsPage();

            //Subscribe to page's close event
            resultsPage.ResultsScreenDismissed += QuizCompleted;

           await Navigation.PushModalAsync(resultsPage);		
        }

		public void QuizCompleted(object sender, EventArgs e)
		{
			DisplayAlert("Congrats!", "Great job finishing Quiz!", "OK");
		}
    }
}
