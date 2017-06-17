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

		async void OnDismissButtonClicked(object sender, EventArgs args)
		{			
            ResultsScreenDismissed(this, EventArgs.Empty);
            await Navigation.PopModalAsync();
		}
	
    }
}
