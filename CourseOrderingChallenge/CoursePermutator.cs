using System;
using System.Collections.Generic;
using CourseOrderingChallenge.Models;

namespace CourseOrderingChallenge
{
    /// <summary>
    ///     Class to generate possibilities (permutations) of completing all courses,
    ///     given the list of courses and its pre-requisites.
    /// </summary>
    public class CoursePermutator
    {
        public List<Course> Courses { get; private set; }

        private Dictionary<int, bool> CompletedCourseDictionary;
        private List<int> TempResultSet;

        /// <summary>
        ///     Create an instance of CoursePermutator object.
        /// </summary>
        /// <param name="courses">
        ///     The list of courses along with its pre-requisites.
        /// </param>
        public CoursePermutator(List<Course> courses)
        {
            // Set values from parameters.
            Courses = courses;

            // Initialize private objects used in this class.
            TempResultSet = new List<int>(courses.Count);
            InitializeCompletedCourseDictionary();
        }

        /// <summary>
        ///     Get all possible sequences of completing the courses,
        ///     adhering to each course's pre-requisite.
        /// </summary>
        /// <returns>
        ///     IEnumerable of IEnmuerable of course ids.
        ///     The outer IEnumerable contains all possible sequence.
        ///     The inner IEnumerable contains a sequence of course ids to complete all the courses.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">
        ///     Thrown when the pre-requisites imposed are not valid.
        ///     This can happen when there is circular dependency of pre-requisites.
        /// </exception>
        public IEnumerable<IEnumerable<int>> GetAllSequences()
        {
            // If we have completed all courses, return this as a result.
            if (TempResultSet.Count == Courses.Count)
            {
                // Note to dev: this condition cannot be combined with the above,
                // As we'd like to avoid executing Else block at this stage.
                if (TempResultSet.Count > 0)
                    yield return new List<int>(TempResultSet);
            }
            else
            {
                // Set a flag to ensure that at least one next course can be pursued.
                // This block will not be called recursively when we are past the last course.
                bool isAnyCourseAvailable = false;

                for (int i = 0; i < Courses.Count; i++)
                {
                    var course = Courses[i];
                    if (!CourseAvailabilityChecker.IsAvailable(CompletedCourseDictionary, course))
                        continue;

                    // If it reaches this part of the for loop,
                    // at least one next course is available to be pursued.
                    isAnyCourseAvailable = true;

                    // Add course id to result set, and call this function recursively,
                    // to calculate all possibilities with this course id.
                    AddToResultSet(course.Id);

                    var sequences = GetAllSequences();
                    foreach (var sequence in sequences)
                        yield return sequence;

                    // Remove last course id from ResultSet, to compute other possibilities.
                    RemoveLastResultSet();
                }

                // Ensures that at least one next course is available to pursue.
                if (!isAnyCourseAvailable)
                {
                    string exceptionMesssage = "Invalid pre-requisites. No way to complete all the courses.";
                    throw new InvalidOperationException(exceptionMesssage);
                }
            }
        }

        /// <summary>
        ///     Helper function to initialize CompletedCourseDictionary.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">
        ///     Thrown when duplicate course id is found.
        /// </exception>
        private void InitializeCompletedCourseDictionary()
        {
            CompletedCourseDictionary = new Dictionary<int, bool>();

            // Add each course into CompletedCourses dictionary, setting the initial value to false.
            foreach (var course in Courses)
            {
                int courseId = course.Id;

                // Ensure there is no duplicate course.
                if (CompletedCourseDictionary.ContainsKey(courseId))
                {
                    string exceptionMessage = string.Format(
                        "Fails to create {0} object. Duplicate course id ({1}) found.",
                        this.GetType()?.Name, course.Id);

                    throw new InvalidOperationException(exceptionMessage);
                }

                CompletedCourseDictionary.Add(courseId, false);
            }
        }


        /// <summary>
        ///     Helper function to add the specified course id to TempResultSet,
        ///     and mark the course id as completed.
        /// </summary>
        /// <param name="courseId">
        ///     The course id to add to the result set.
        /// </param>
        private void AddToResultSet(int courseId)
        {
            TempResultSet.Add(courseId);
            CompletedCourseDictionary[courseId] = true;
        }

        /// <summary>
        ///     Helper function to remove last course id from TempResultSet,
        ///     and mark the course id as not completed.
        /// </summary>
        private void RemoveLastResultSet()
        {
            // Get courseId at last index position.
            int lastIndex = TempResultSet.Count - 1;
            int courseId = TempResultSet[lastIndex];

            // Remove last value from result set, setting the course completion to false.
            TempResultSet.RemoveAt(lastIndex);
            CompletedCourseDictionary[courseId] = false;
        }
    }
}