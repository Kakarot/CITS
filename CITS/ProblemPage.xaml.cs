using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace CITS
{
    public partial class ProblemPage : ContentPage
    {
        public event EventHandler SolutionSubmitted;
        public event EventHandler HintRequested;
        private int currentHintNumber = 0;
        public String ProblemNumber
        {
            get;
            set;
        }
		public String Problem
		{
			get;
			set;
		}

        public String Solution
        {
            get;
            set;
        }
		public Entry SolutionTextField
		{
            get;
			set;
		}

        public ProblemPage()
        {
            InitializeComponent();
        }

        public List<String> Hints
        {
            get;
            set;
        }

        void Handle_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
         //   throw new NotImplementedException();
        }

		void OnSubmitButtonClicked(object sender, EventArgs args)
		{
            this.Solution = SolutionEntry.Text;
            SolutionSubmitted(this, EventArgs.Empty);         
			//await Navigation.PopModalAsync();
		}

       public void UpdateProblemPage()
        {
            ProblemNumberLabel.Text = ProblemNumber;
            ProblemLabel.Text = Problem;
            SolutionEntry.Text="";
            currentHintNumber = 0;
            SolutionTextField = SolutionEntry;
        }

		void OnHintButtonClicked(object sender, EventArgs args)
		{
            if (currentHintNumber < Hints.Count)
            {
                DisplayAlert("Hint", this.Hints[currentHintNumber], "OK");
                currentHintNumber++;
                HintRequested(this, EventArgs.Empty);
            }
            else
            {
                DisplayAlert("Hint", "There are no more hints for this problem", "OK");
            }
		}
    }
}
