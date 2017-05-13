using System.Collections.Generic;

namespace CourseOrderingChallenge.Models
{
    /// <summary>
    ///     Model class for course data.
    /// </summary>
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public HashSet<int> Prerequisites { get; set; }

        public Course()
        {
            this.Prerequisites = new HashSet<int>();
        }
    }
}