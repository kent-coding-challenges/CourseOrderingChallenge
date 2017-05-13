using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Linq;
using CourseOrderingChallenge.Models;
using CourseOrderingChallenge.Tests.MockupFactory;

namespace CourseOrderingChallenge.Tests
{
    [TestClass]
    public class CoursePermutatorTests
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Constructor_OnDuplicateCourses()
        {
            // Build mockup object with duplicate entry.
            var factory = new CourseMockupFactory();
            var course = factory.BuildDummy(1);

            var courses = new List<Course>();
            courses.Add(course);
            courses.Add(course);

            var permutator = new CoursePermutator(courses);
        }

        [TestMethod]
        public void GetAllSequences_EnsureCorrectOrderSequence()
        {
            var factory = new CourseMockupFactory();
            for (int i = 1; i <= 7; i++)
            {
                // Build mockup list with size = i.
                var courses = factory.BuildDummyList(i);
                factory.SetPrerequisiteAsFirstCourse(ref courses);

                var permutator = new CoursePermutator(courses);
                var sequences = permutator.GetAllSequences();

                foreach (var course in courses)
                {
                    var prerequisites = course.Prerequisites;

                    foreach (var order in sequences)
                    {
                        bool isCourseFound = false;
                        foreach (var courseId in order.ToList())
                        {
                            if (courseId == course.Id)
                                isCourseFound = true;

                            if (isCourseFound && prerequisites.Contains(courseId))
                                Assert.Fail();
                        }
                    }
                }
            }
        }

        [TestMethod]
        public void GetAllSequences_EnsureCorrectOrderLength()
        {
            var factory = new CourseMockupFactory();
            for (int i = 1; i <= 7; i++)
            {
                // Build mockup list with size = i.
                var courses = factory.BuildDummyList(i);
                factory.SetPrerequisiteAsFirstCourse(ref courses);

                var permutator = new CoursePermutator(courses);
                var sequences = permutator.GetAllSequences();

                foreach (var order in sequences)
                {
                    // Each order needs to have the same length as the total courses length.
                    if (order.Count() != i)
                        Assert.Fail();
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetAllSequences_OnNoCompletionPossible()
        {
            var factory = new CourseMockupFactory();

            // Build mockup list with size = n.
            int n = 5;
            var courses = factory.BuildDummyList(n);

            // Build circular pre-requisites (invalid pre-requisites).
            factory.SetPrerequisiteAsFirstCourse(ref courses);
            courses[0].Prerequisites.Add(courses[1].Id);

            var permutator = new CoursePermutator(courses);
            var sequences = permutator.GetAllSequences().ToList();
        }
    }
}
