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

        void OnDismissButtonClicked(object sender, EventArgs args)
		{			
            ResultsScreenDismissed(this, EventArgs.Empty);

		}

        public void UpdatePage()
        {
            this.ResultsTotalLabel.Text = ResultsSummary;
            this.ResultsGeneratedProblemsLabel.Text = NumberOfGeneratedProblems;
        }
	
    }
}
