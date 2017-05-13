using System;
using System.Collections.Generic;
using CourseOrderingChallenge.Models;

namespace CourseOrderingChallenge
{
    /// <summary>
    ///     Contains functionalities to check course availability.
    /// </summary>
    public static class CourseAvailabilityChecker
    {
        /// <summary>
        ///     Checks whether the course is available.
        /// </summary>
        /// <param name="completedCourseDictionary">
        ///     Dictionary with courseId as key and completed boolean as value.
        /// </param>
        /// <param name="course">
        ///     The course to check its availability.
        /// </param>
        /// <returns>
        ///     True if course is available and not completed yet, false otherwise.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown when any parameters passed is null.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        ///     Thrown when the pre-requisites imposed are not valid.
        ///     This can happen when a pre-requisite to an invalid course id is specified.
        /// </exception>
        public static bool IsAvailable(Dictionary<int, bool> completedCourseDictionary, Course course)
        {
            if (completedCourseDictionary == null || course == null)
                throw new ArgumentNullException();

            int courseId = course.Id;

            // If course is already completed, return false.
            if (completedCourseDictionary[courseId])
                return false;

            var prerequisites = course.Prerequisites;
            foreach (int id in prerequisites)
            {
                // If course id doesn't exist in dictionary, pre-requisite refers to an invalid course id.
                if (!completedCourseDictionary.ContainsKey(id))
                {
                    string exceptionMessage = string.Format(
                        "Invalid pre-requisites. Course id ({0}) is not found.", id);
                    throw new InvalidOperationException(exceptionMessage);
                }

                // If one of the pre-requisites have not been completed, course is not available yet.
                if (!completedCourseDictionary[id])
                    return false;
            }

            // If course has no pre-requisite, or all pre-requisites have been completed,
            // Then this course is available.
            return true;
        }
    }
}