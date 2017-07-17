using System;
namespace CITS
{
    public class EmotionEventArgs : EventArgs
    {
        public double Anger
        {
            get;
            set;
        }
		public double Contempt
		{
			get;
			set;
		}

		public double Disgust
		{
			get;
			set;
		}

		public double Fear
		{
			get;
			set;
		}

		public double Happiness
		{
			get;
			set;
		}

		public double Neutral
		{
			get;
			set;
		}

		public double Sadness
		{
			get;
			set;
		}

		public double Surprise
		{
			get;
			set;
		}

		public EmotionEventArgs(float anger, float contempt, float disgust,
                                  float fear, float happiness, float neutral,
                                  float sadness, float surprise)
        {
            this.Anger = anger;
            this.Contempt = contempt;
            this.Disgust = disgust;
            this.Fear = fear;
            this.Happiness = happiness;
            this.Neutral = neutral;
            this.Sadness = sadness;
            this.Surprise = surprise;
        }
    }
}
