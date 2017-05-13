using CsvHelper.Configuration;
using CourseOrderingChallenge.Models;

namespace CourseOrderingChallenge.CsvHelper.Mapper
{
    public class CoursePrerequisiteMap : CsvClassMap<CoursePrerequisite>
    {
        public CoursePrerequisiteMap()
        {
            Map(m => m.CourseId).Name("course");
            Map(m => m.RequireCourseId).Name("prerequisite");
        }
    }
}
