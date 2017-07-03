using System;
using System.Data;
using CITS.Models;
using System.Collections.Generic;
namespace CITS
{
    public class ProblemGenerator
    {
        public static Random rng;
        public ProblemGenerator()
        {
            rng = new Random();
        }


        public Queue<MathProblemModel> GenerateOrderOfOperationProblems(uint numberOfProblems)
        {
            var queueOfMathProblems = new Queue<MathProblemModel>();

            //Generate number of problems requested
            for (uint i = 0; i < numberOfProblems; i++)
            {
                queueOfMathProblems.Enqueue(GenerateOrderOfOperationProblem());
            }

            return queueOfMathProblems;
        }

        private MathProblemModel GenerateOrderOfOperationProblem()
        {
            //We have a string place holders that are randomly chosen
            //then we have those place holders randomly filled with
            //numbers 0-12

            var generatedExpression = String.Empty;


            //Increment this random range everytime we add a new place holder string
            var stringTemplate = new Random().Next(1, 9);

            var tempSubtractVariable = 1;
            switch (stringTemplate)
            {
                case 1:
                    generatedExpression =
                String.Format("{0} * ({1} + {2})",
                              rng.Next(0, 13),
                              rng.Next(0, 13),
                              rng.Next(0, 13)
                             );
                    break;

                case 2:
                    generatedExpression =
                    String.Format("{0} + ({1} * {2})",
                               rng.Next(0, 13),
                               rng.Next(0, 13),
                               rng.Next(0, 13)
                              );
                    break;

                case 3:
                    generatedExpression =
                    String.Format("{0} + ({1} + {2})",
                               rng.Next(0, 13),
                               rng.Next(0, 13),
                               rng.Next(0, 13)
                              );
                    break;

                case 4:
                    generatedExpression =
                    String.Format("{0} + {1} * {2}",
                               rng.Next(0, 13),
                               rng.Next(0, 13),
                               rng.Next(0, 13)
                              );
                    break;

                case 5:
                    generatedExpression =
                        String.Format("({0} * {1}) + {2}",
                               rng.Next(0, 13),
                               rng.Next(0, 13),
                               rng.Next(0, 13)
                              );
                    break;

                case 6:
                    tempSubtractVariable = rng.Next(1, 13);
                    generatedExpression =
                        String.Format("({0} - {1}) * {2}",
                               tempSubtractVariable,
                               rng.Next(0, tempSubtractVariable),
                               rng.Next(0, 13)
                              );
                    break;

                case 7:
                    tempSubtractVariable = rng.Next(1, 13);
                    generatedExpression =
                    String.Format("{0} + ({1} - {2})",
                               rng.Next(0, 13),
                               tempSubtractVariable,
                               rng.Next(0, tempSubtractVariable)
                              );
                    break;

                case 8:
                    tempSubtractVariable = rng.Next(1, 13);
                    generatedExpression =
           String.Format("{0} * ({1} - {2})",
                         rng.Next(0, 13),
                         tempSubtractVariable,
                         rng.Next(0, tempSubtractVariable)
                        );
                    break;
            }


            var hintList = new List<string>();
            hintList.Add("Please Excuse My Dear Aunt Sally");
            hintList.Add("Solve the expressions within parentheses first");
            hintList.Add("Addition has lower precedence than Multiplication");
            hintList.Add("Subtraction has lower precedence than Multiplication");
            hintList.Add("Solve numbers within parentheses first, then move on to multiplication");


            return new MathProblemModel(generatedExpression, Convert.ToString((int)new DataTable().Compute(generatedExpression, "")), hintList);

        }
    }
}
