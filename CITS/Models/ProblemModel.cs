using System;
namespace CITS.Models
{
    public class ProblemModel : IProblem
    {
        public ProblemModel(string Problem, string Solution) 
        {
            this.Problem = Problem;
            this.Solution = Solution;
        }

        public string Problem { get; set; }
        public string Solution { get; set; }



        public virtual bool IsSolutionCorrect(string candidateSolution)
        {
            throw new NotImplementedException();
        }
    }
}
