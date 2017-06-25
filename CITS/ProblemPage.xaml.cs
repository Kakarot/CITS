using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace CITS
{
    public partial class ProblemPage : ContentPage
    {
        public event EventHandler SolutionSubmitted;
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

        public ProblemPage()
        {
            InitializeComponent();
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
        }
    }
}
