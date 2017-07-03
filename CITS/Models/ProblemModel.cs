using System;
using System.Collections.Generic;
namespace CITS.Models
{
    public class ProblemModel : IProblem
    {
        public ProblemModel(string Problem, string Solution, List<String> listOfHints) 
        {
            this.Problem = Problem;
            this.Solution = Solution;
            this.Hints = listOfHints;
        }

        public string Problem { get; set; }
        public string Solution { get; set; }
        public List<String> Hints { get; set; }


        public virtual bool IsSolutionCorrect(string candidateSolution)
        {
            throw new NotImplementedException();
        }
    }
}
