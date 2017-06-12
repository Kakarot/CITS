using System;
namespace CITS.Models
{
    public interface IProblem
    {
        string Problem { get; set; }
        string Solution { get; set; }
        Boolean IsSolutionCorrect(String candidateSolution);
    }
}
