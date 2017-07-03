using System;
using System.Collections.Generic;
namespace CITS.Models
{
    public class MathProblemModel : ProblemModel
    {
        public MathProblemModel(string problem, string solution, List<String> listOfHints):base(problem,solution, listOfHints){}

        public override Boolean IsSolutionCorrect(String candidateSolution)
        {
            return Solution.Equals(candidateSolution.Trim());
        }
    }
}
