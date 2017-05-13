using System;
using System.Collections.Generic;
using System.IO;
using CourseOrderingChallenge.CsvHelper.Mapper;
using CourseOrderingChallenge.CsvHelper.Reader;
using CourseOrderingChallenge.Models;

namespace CourseOrderingChallenge.TestRunner
{
    /// <summary>
    ///     Handlles input reading from input/output files in TestCases folders.
    /// </summary>
    public class TestInputReader
    {
        static string InputDirectoryStructure = "TestCases\\Test";

        static string InputCourseFileName = "courses.csv";
        static string InputPrerequisiteFileName = "prerequisites.csv";
        static string ExpectedOutputFileName = "expected-output.txt";
        static string messageFileName = "message.txt";
        
        private string TestCaseDirectory;
        private int TestNumber;

        /// <summary>
        ///     Create a new instance of TestInputReader.
        ///     This will initialize the test number and the corresponding directory path.
        /// </summary>
        /// <param name="number"></param>
        public TestInputReader(int number)
        {
            TestNumber = number;
            BuildBasePath();
        }

        /// <summary>
        ///     Read courses.csv and return list of courses in this file.
        /// </summary>
        public List<Course> ReadCourseList()
        {
            string inputCourseFilePath = GetFilePath(InputCourseFileName);
            string csvText = File.ReadAllText(inputCourseFilePath);

            var reader = new CsvObjectReader<Course, CourseMap>();
            return reader.GetList(csvText);
        }

        /// <summary>
        ///     Read prerequisites.csv and return list of prerequisites in this file.
        /// </summary>
        public List<CoursePrerequisite> ReadPrerequisiteList()
        {
            string inputPrerequisiteFilePath = GetFilePath(InputPrerequisiteFileName);
            string csvText = File.ReadAllText(inputPrerequisiteFilePath);

            var reader = new CsvObjectReader<CoursePrerequisite, CoursePrerequisiteMap>();
            return reader.GetList(csvText);
        }

        /// <summary>
        ///     Read expected-output.txt and convert into list of possibilities,
        ///     where each possibility contains an IEnumerable of integers.
        /// </summary>
        /// <exception cref="System.IO.FileNotFoundException">
        ///     Thrown when expected-output.txt contains invalid integer value.
        /// </exception>
        public List<IEnumerable<int>> ReadExpectedOutput()
        {
            // Read expected output text.
            string expectedOutputFilePath = GetFilePath(ExpectedOutputFileName);
            string[] expectedOutput = File.ReadAllLines(expectedOutputFilePath);

            var output = new List<IEnumerable<int>>();
            foreach (var line in expectedOutput)
            {
                // Add each comma-separated line as IEnumerable<int>.
                string[] values = line.Split(',');

                var list = new List<int>();
                foreach (var valueText in values)
                {
                    int value;

                    // Ensure each value in the comma-separated line is a valid integer.
                    if (!Int32.TryParse(valueText, out value))
                        throw new InvalidCastException("Expected output contains invalid character.");

                    list.Add(value);
                }

                output.Add(list);
            }

            return output;
        }

        /// <summary>
        ///     Read the test case header message supplied in message.txt.
        /// </summary>
        public string ReadHeaderMessage()
        {
            string messageFilePath = GetFilePath(messageFileName);
            return File.ReadAllText(messageFilePath);
        }

        /// <summary>
        ///     Helper function to get file path of the specified fileName in base directory.
        /// </summary>
        /// <exception cref="System.IO.FileNotFoundException">
        ///     Thrown when the specified fileName is not found in base directory.
        /// </exception>
        private string GetFilePath(string fileName)
        {
            string filePath = Path.Combine(TestCaseDirectory, fileName);

            // Ensure file exists.
            if (!File.Exists(filePath))
            {
                string exceptionMessage = string.Format("{0} not found in {1} directory.",
                    fileName, TestCaseDirectory);
                throw new FileNotFoundException(exceptionMessage);
            }

            return filePath;
        }

        /// <summary>
        ///     Helper function to build base directory path.
        /// </summary>
        /// <exception cref="System.IO.DirectoryNotFoundException">
        ///     Thrown when the base directory is not found in the running app folder.
        /// </exception>
        private void BuildBasePath()
        {
            var appDomain = AppDomain.CurrentDomain;
            var appPath = appDomain.RelativeSearchPath ?? appDomain.BaseDirectory;
            string directory = string.Format("{0}{1}", InputDirectoryStructure, TestNumber);

            // Ensure directory exists.
            if(!Directory.Exists(directory))
            {
                string exceptionMessage = string.Format("{0} directory not found.", TestCaseDirectory);
                throw new DirectoryNotFoundException(exceptionMessage);
            }
            
            TestCaseDirectory = Path.Combine(appPath, directory);
        }
    }
}
