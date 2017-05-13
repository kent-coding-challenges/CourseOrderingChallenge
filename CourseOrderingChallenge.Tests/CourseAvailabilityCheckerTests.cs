using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using CourseOrderingChallenge.Models;
using CourseOrderingChallenge.Tests.MockupFactory;

namespace CourseOrderingChallenge.Tests
{
    [TestClass]
    public class CourseAvailabilityCheckerTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IsAvailable_OnNullDictionaryParameter()
        {
            // Pass null completedCourseDictionary parameter.
            CourseAvailabilityChecker.IsAvailable(null, new Course());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IsAvailable_OnNullCourseParameter()
        {
            // Pass null course parameter.
            CourseAvailabilityChecker.IsAvailable(new Dictionary<int, bool>(), null);
        }

        [TestMethod]
        public void IsAvailable_OnNoPrerequisite()
        {
            // Build mockup objects with no pre-requisites.
            var factory = new CourseMockupFactory();
            var course = factory.BuildDummy(1);

            var completedCourseDictionary = new Dictionary<int, bool>();
            completedCourseDictionary.Add(course.Id, false);

            // If course is not completed and has no pre-requisite, should return true.
            Assert.IsTrue(CourseAvailabilityChecker.IsAvailable(completedCourseDictionary, course));

            // If course is completed, should return false.
            completedCourseDictionary[course.Id] = true;
            Assert.IsFalse(CourseAvailabilityChecker.IsAvailable(completedCourseDictionary, course));
        }

        [TestMethod]
        public void IsAvailable_OnIncompletePrerequisite()
        {
            // Build mockup objects with no pre-requisites.
            int n = 5;
            var factory = new CourseMockupFactory();
            var courses = factory.BuildDummyList(5);
            factory.SetPrerequisiteAsFirstCourse(ref courses);

            // Set that no course has been completed.
            var completedCourseDictionary = new Dictionary<int, bool>();
            for (int i = 0; i < courses.Count; i++)
                completedCourseDictionary.Add(courses[i].Id, false);

            // As all courses requires first course id (1) and given that no course has been completed,
            // The second to last course should return false, while the first one should return true.
            Assert.IsTrue(CourseAvailabilityChecker.IsAvailable(completedCourseDictionary, courses[0]));
            for (int i = 1; i < n; i++)
            {
                Assert.IsFalse(CourseAvailabilityChecker.IsAvailable(completedCourseDictionary, courses[i]));
            }

            // Second case, try for partial pre-requisite match.
            // Set course 3 (index: 2) to require course 2 (on top of 1).
            int targetIndex = 2;
            courses[targetIndex].Prerequisites.Add(2);

            // Mark course 1 as completed.
            completedCourseDictionary[1] = true;

            // As course 3 requires 1 and 2, and only 1 is completed, this should return false.
            Assert.IsFalse(CourseAvailabilityChecker.IsAvailable(completedCourseDictionary, courses[targetIndex]));
        }
    }
}
