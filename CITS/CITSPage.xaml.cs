using Xamarin.Forms;

namespace CITS
{
    public partial class CITSPage : MasterDetailPage  
    {
        
        public CITSPage()
        {
            InitializeComponent();
            Detail = new NavigationPage(new OrderOfOperationsPage());
            IsPresented = false;
        }

        void Handle_Clicked_Order_Of_Operations(object sender, System.EventArgs e)
        {
            Detail = new NavigationPage(new OrderOfOperationsPage());
            IsPresented = false;
        }
    }
}
