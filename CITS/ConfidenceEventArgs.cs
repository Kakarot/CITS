using System;
namespace CITS
{
    public class ConfidenceEventArgs : EventArgs
    {
        public string ConfidenceAverage
        {
            get;
            set;
        }
        public ConfidenceEventArgs(double confidenceAverage)
        {
            this.ConfidenceAverage = string.Format("{0:P}", confidenceAverage);
        }
    }
}
