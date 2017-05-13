using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using CourseOrderingChallenge.Models;

namespace CourseOrderingChallenge.TestRunner
{
    /// <summary>
    ///     Class to run test cases from TestCases/Test{i} folder.
    /// </summary>
    public class TestCaseRunner
    {
        private TestInputReader InputReader;

        private int TestNumber;
        private string TestMessage;

        private List<Course> Courses;
        private List<IEnumerable<int>> ExpectedOutput;

        /// <summary>
        ///     Initialize a new TestCaseRunner object.
        /// </summary>
        /// <param name="number">
        ///     The test case number.
        ///     This should correspond with {i} in TestCases/Test{i} folder.
        /// </param>
        public TestCaseRunner(int number)
        {
            TestNumber = number;

            // Read input/output data from file.
            InputReader = new TestInputReader(number);
            TestMessage = InputReader.ReadHeaderMessage();
            ExpectedOutput = InputReader.ReadExpectedOutput();

            // Read the course and pre-requisites, and set its relationship.
            SetCoursesAndPrerequisites();
        }

        /// <summary>
        ///     Run the test case specified by the TestNumber.
        /// </summary>
        public void RunTest()
        {
            // Initialize new Stopwatch instance to measure the execution time of this test case.
            var sw = new Stopwatch();

            // All output from test case will be appended to following StringBuilder object.
            var outputBuilder = new StringBuilder();
            
            try
            {
                // Start stopwatch timer.
                sw.Start();

                // Compute results, calling .ToList() to get all results before proceeding.
                var permutator = new CoursePermutator(Courses);
                var sequences = permutator.GetAllSequences().ToList();

                // End stopwatch timer.
                sw.Stop();

                Console.WriteLine("Successfully run test case. Here's the result.");
                    
                // Output each result.
                foreach (var sequence in sequences)
                {
                    // Store reference to last courseId in this sequence.
                    int lastCourseId = sequence.Last();

                    foreach (var courseId in sequence)
                    {
                        // Don't print comma for last sequence.
                        string format = (courseId == lastCourseId) ? "{0}" : "{0},";

                        string output = string.Format(format, courseId);
                        Console.Write(output);
                    }

                    // Print next line for separation between each sequence.
                    Console.WriteLine();
                }

                // Verify the output.
                var verifier = new TestCaseVerifier();
                bool isCorrectOutput = verifier.Verify(sequences, ExpectedOutput);

                // Print the result of the output matched against the ExpectedOutput.
                outputBuilder.AppendLine();
                if (isCorrectOutput)
                    outputBuilder.AppendLine("PASSED - match expected output.");
                else
                    outputBuilder.AppendLine("FAILED - doesn't match expected output.");
            }
            catch (Exception exception)
            {
                // End stopwatch timer (as exception was thrown and was not stopped before).
                sw.Stop();

                // If this block is executed, exception has been thrown.
                // Print the details of the exception.
                string log = string.Format("Exception has been thrown.\n{0}", exception.Message);
                outputBuilder.AppendLine(log);
            }
            
            // Set the execution time of this test case for output.
            outputBuilder.AppendFormat("\nComputation time: {0}ms.\n", sw.ElapsedMilliseconds);

            // Write the output built so far.
            Console.WriteLine(outputBuilder.ToString());
        }

        /// <summary>
        ///     Write the TestCase header to Console.
        /// </summary>
        public void WriteTestHeader()
        {
            Console.WriteLine("--------------------");
            Console.WriteLine(string.Format("Test Case {0}", TestNumber));
            Console.WriteLine(TestMessage);
            Console.WriteLine("--------------------\n");
        }

        /// <summary>
        ///     Helper method to set courses list, as well as configuring the pre-requisites relationship.
        /// </summary>
        private void SetCoursesAndPrerequisites()
        {
            Courses = InputReader.ReadCourseList();
            var prerequisites = InputReader.ReadPrerequisiteList();

            // Add each pre-requisite in the relevant Course model.
            for (int i = 0; i < Courses.Count; i++)
            {
                foreach (var prerequisite in prerequisites)
                {
                    // If pre-requisite is for this course, then assign requireCourseId.
                    if (prerequisite.CourseId == Courses[i].Id)
                    {
                        Courses[i].Prerequisites.Add(prerequisite.RequireCourseId);
                    }
                }
            }
        }
    }
}