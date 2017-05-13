using System.Collections.Generic;
using System.Linq;
using CourseOrderingChallenge.Models;

namespace CourseOrderingChallenge.Tests.MockupFactory
{
    public class CourseMockupFactory
    {
        /// <summary>
        ///     Build dummy Course object with the specified id.
        /// </summary>
        public Course BuildDummy(int id)
        {
            return new Course()
            {
                Id = id,
                Name = string.Format("Course-{0}", id)
            };
        }

        /// <summary>
        ///     Build dummy List<Course> object with course id from 1 to size.
        /// </summary>
        public List<Course> BuildDummyList(int size)
        {
            var list = new List<Course>();
            for (int i = 1; i <= size; i++)
            {
                var course = BuildDummy(i);
                list.Add(course);
            }
            return list;
        }

        /// <summary>
        ///     Set all courses except the first course to have first course as a pre-requisite.
        /// </summary>
        public void SetPrerequisiteAsFirstCourse(ref List<Course> courses)
        {
            int firstCourseId = courses.First().Id;
            for (int i = 1; i < courses.Count; i++)
            {
                courses[i].Prerequisites = new HashSet<int>() { firstCourseId };
            }
        }
    }
}
