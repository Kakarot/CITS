using System;
using System.Collections.Generic;
namespace CITS.Models
{
    public class QuizModel
    {
        public static ProblemGenerator _problemGenerator;

        public int NumberOfProblems
        {
            get;
            set;
        }

        public int NumberOfErrors
        {
            get;
            set;

        }

        public int NumberOfCorrectAnswers
        {
            get;
            set;
        }

        public int NumberOfGeneratedProblems
        {
            get;
            set;
        }

        public Boolean WasPreviousAnswerCorrect
        {
            get;
            set;
        }

        public Double ConfidenceAverage
        {
            get {if (_numberOfConfidenceEvaluations == 0) return 0;
                else return ((_happiness + _neutral + _surprise)/_numberOfConfidenceEvaluations); }
        }

        /*
         * Emotion variables are stored in quiz object to monitor the average
         * emotions exprienced during quiz duration.
         */
        private double _anger;

        private double _contempt;

        private double _disgust;

        private double _fear;

        private double _happiness;

        public double _neutral;

        public double _sadness;

        public double _surprise;

        private int _numberOfConfidenceEvaluations;

        public QuizModel()
        {
            this._anger = 0;
            this._contempt = 0;
            this._disgust = 0;
            this._neutral = 0;
            this._sadness = 0;
            this._surprise = 0;
            _numberOfConfidenceEvaluations = 0;
            _problemGenerator = new ProblemGenerator();
        }


        public Queue<MathProblemModel> GenerateMathProblems(uint numberOfProblems)
        {
            var mpmQueue = _problemGenerator.GenerateOrderOfOperationProblems(numberOfProblems);
            this.NumberOfProblems += mpmQueue.Count;
            return mpmQueue;
        }

        public String GetRecommendation()
        {
            string recommendation;
            if (NumberOfProblems != 0)
            {
                var percentCorrect = NumberOfCorrectAnswers / NumberOfProblems;

                if(percentCorrect > 0.80d)
                {
                    recommendation = "Analysis:\n\nYou have mastered this material!";
				}
                else
                {
                    recommendation = "Analysis:\n\nYou have not mastered this subject yet.";   
                }
            }
            else
            {
                recommendation = "You cannot ask for a recommendation when there were no problems given!";   
            }
            return recommendation;
        }

        public Boolean GenerateNewProblemBasedOnConfidence(EmotionEventArgs e)
        {
            //Sum up emotions that could infer unconfidence
            double unConfidence = e.Anger + e.Contempt + e.Fear + e.Sadness + e.Disgust;
            double confidence = e.Happiness + e.Neutral + e.Surprise;

            Boolean generateNewProblem = false;

            if (confidence <= unConfidence)
            {
                //Multiply 100 to get percentage and then truncate to integer
                int oddsToGenerateNewProblem = (int)(100 * (unConfidence));
                int randomNumber = ProblemGenerator.rng.Next(1, 101);
                if(randomNumber <= oddsToGenerateNewProblem)
                {
                    generateNewProblem = true;
                }
            }

            return generateNewProblem;
        }

		public void AddEmotionsForLastSolvedProblem(double anger, double contempt, double disgust,
								  double fear, double happiness, double neutral,
								  double sadness, double surprise)
        {
            this._anger += anger;
            this._contempt += contempt;
            this._disgust += disgust;
            this._fear += fear;
            this._happiness += happiness;
            this._neutral += neutral;
            this._sadness += sadness;
            this._surprise += surprise;

            //increment the number of times emotion values have been summed
            //will be used for averaging emotions
            _numberOfConfidenceEvaluations++;
        }

       
    }
}
