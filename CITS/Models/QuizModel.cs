using System;
using System.Collections.Generic;
namespace CITS.Models
{
    public class QuizModel
    {
        public static ProblemGenerator _problemGenerator = new ProblemGenerator();

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

        public QuizModel()
        {
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
                    recommendation = "Recommendation:\n\nYou are prepared for the real thing!";
				}
                else
                {
                    recommendation = "Recommendation:\n\nTake the quiz again tomorrow.";   
                }
            }
            else
            {
                recommendation = "You cannot ask for a recommendation when there were no problems given!";   
            }
            return recommendation;
        }
    }
}
