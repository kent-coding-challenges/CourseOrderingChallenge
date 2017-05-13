using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;

namespace CourseOrderingChallenge.CsvHelper.Reader
{
    /// <summary>
    ///     Helper class to read csv files as list of strongly typed object.
    /// </summary>
    /// <typeparam name="T">
    ///     Type of object of the csv file.
    /// </typeparam>
    /// <typeparam name="TMap">
    ///     The CsvClassMap used to translate csv header columns to the specified T object.
    /// </typeparam>
    public class CsvObjectReader<T, TMap>
        where TMap : CsvClassMap
    {
        /// <summary>
        ///     Convert the csv text passed into List<T>, using TMap as the mapper.
        /// </summary>
        /// <exception cref="System.InvalidCastException">
        ///     Thrown when csv file contains missing fields.
        /// </exception>
        public List<T> GetList(string csvText)
        {
            var result = new List<T>();
            using (StringReader reader = new StringReader(csvText))
            {
                try
                {
                    CsvReader csv = new CsvReader(reader);
                    csv.Configuration.WillThrowOnMissingField = true;
                    csv.Configuration.RegisterClassMap<TMap>();
                    result = csv.GetRecords<T>().ToList();
                }
                catch (InvalidCastException exception)
                {
                    throw exception;
                }
            }

            return result;
        }
    }
}
