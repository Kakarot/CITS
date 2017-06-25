using System;
using System.Collections.Generic;
namespace CITS.Models
{
    public class QuizModel
    {
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

        public QuizModel()
        {
        }


        public Queue<MathProblemModel> GenerateMathProblems()
        {
            var mpmList = new Queue<MathProblemModel>();
            mpmList.Enqueue(new MathProblemModel("(2+2)*10+2","42"));
            mpmList.Enqueue(new MathProblemModel("3*(9+2)*2", "66"));
            mpmList.Enqueue(new MathProblemModel("3*9+2*2", "31"));

            this.NumberOfProblems = mpmList.Count;
            return mpmList;
        }

        public String GetRecommendation()
        {
            string recommendation;
            if (NumberOfProblems != 0)
            {
                var percentCorrect = NumberOfCorrectAnswers / NumberOfProblems;

                if(percentCorrect > 0.80d)
                {
                    recommendation = "Recommendation: You are prepared for the real thing!";
				}
                else
                {
                    recommendation = "Recommendation: Take the quiz again tomorrow.";   
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
