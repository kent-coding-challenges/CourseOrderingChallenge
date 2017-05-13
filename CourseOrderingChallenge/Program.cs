using System;
using CourseOrderingChallenge.TestRunner;

namespace CourseOrderingChallenge
{
    /// <summary>
    ///     Run this project to cover black-box testing for this challenge.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            // Run test cases.
            int numberOfTests = 6;
            for (int i = 1; i <= numberOfTests; i++)
            {
                var testCaseRunner = new TestCaseRunner(i);

                // Display the test case header.
                testCaseRunner.WriteTestHeader();

                Console.WriteLine("Press enter to run this test case.");
                Console.ReadLine();

                // Run the test case.
                testCaseRunner.RunTest();

                if (i < numberOfTests)
                {
                    Console.WriteLine("Voila.\nPress enter to clear console and print next test case.");
                    Console.ReadLine();
                    Console.Clear();
                }
            }

            // Prevent console window from closing immediately.
            Console.WriteLine("Press enter to exit this application. Thank you.");
            Console.ReadLine();
        }
    }
}
