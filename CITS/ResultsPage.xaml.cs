using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace CITS
{
    public partial class ResultsPage : ContentPage
    {
        
    public event EventHandler ResultsScreenDismissed;
    
        public ResultsPage()
        {
            InitializeComponent();
        }

        public String ResultsSummary
        {
            get;
            set;
        }

		public String NumberOfGeneratedProblems
		{
			get;
			set;
		}

		public String Recommendation
		{
			get;
			set;
		}

        public Boolean DidPassQuiz
        {
            get;
            set;
        }

        void OnDismissButtonClicked(object sender, EventArgs args)
		{			
            ResultsScreenDismissed(this, EventArgs.Empty);

		}

        public void UpdatePage()
        {
            this.ResultsTotalLabel.Text = ResultsSummary;
            this.ResultsGeneratedProblemsLabel.Text = NumberOfGeneratedProblems;
            this.RecommendationLabel.Text = Recommendation;
            this.ConfidenceAverageLabel.Text = @"{Calculating Confidence Average...}";
            if(DidPassQuiz)
            {
                this.RecommendationLabel.TextColor = Color.Blue;
            }
            else
            {
                this.RecommendationLabel.TextColor = Color.Red;
            }
        }

        public void UpdateConfidence(object sender, ConfidenceEventArgs e)
        {
			this.ConfidenceAverageLabel.Text = "Confidence Level: "+e.ConfidenceAverage;
        }
	
    }
}
