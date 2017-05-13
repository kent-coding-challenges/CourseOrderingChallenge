namespace CourseOrderingChallenge.Models
{
    /// <summary>
    ///     Model class for course pre-requisite data.
    /// </summary>
    public class CoursePrerequisite
    {
        public int CourseId { get; set; }
        public int RequireCourseId { get; set; }
    }
}
