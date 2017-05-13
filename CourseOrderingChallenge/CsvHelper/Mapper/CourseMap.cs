using CsvHelper.Configuration;
using CourseOrderingChallenge.Models;

namespace CourseOrderingChallenge.CsvHelper.Mapper
{
    public class CourseMap : CsvClassMap<Course>
    {
        public CourseMap()
        {
            Map(m => m.Id).Name("id");
            Map(m => m.Name).Name("title");
        }
    }
}
