using System.Collections.Generic;
using System.Linq;

namespace CourseOrderingChallenge.TestRunner
{
    /// <summary>
    ///     Helper class to verify output from test cases.
    /// </summary>
    public class TestCaseVerifier
    {
        /// <summary>
        ///     Given list of list of course Ids' output and expected output,
        ///     Verify that both match, ignoring the result order.
        /// </summary>
        /// <returns>
        ///     True if output is verified (correct), false otherwise.
        /// </returns>
        public bool Verify(List<IEnumerable<int>> output, List<IEnumerable<int>> expectedOutput)
        {
            bool isCorrectOutput = output.Count == expectedOutput.Count;
            if (isCorrectOutput)
            {
                foreach (var outputSequence in output)
                {
                    bool existsInOutput = false;
                    foreach (var expectedOutputSequence in expectedOutput)
                    {
                        if (expectedOutputSequence.SequenceEqual(outputSequence))
                        {
                            existsInOutput = true;
                        }
                    }

                    if (!existsInOutput)
                    {
                        isCorrectOutput = false;
                        break;
                    }
                }
            }

            return isCorrectOutput;
        }
    }
}
