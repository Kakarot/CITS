using System;
namespace CITS.Models
{
    public class MathProblemModel : ProblemModel
    {
        public MathProblemModel(string Problem, string Solution):base(Problem,Solution){}

        public override Boolean IsSolutionCorrect(String candidateSolution)
        {
            return Solution.Equals(candidateSolution.Trim());
        }
    }
}
