using System.Collections.Generic;
using System.IO;
using TinyCsvParser.Model;

namespace TinyCsvParser.CsvDataConverter

{
    public interface IDataConverter
    {
        ITable Converter(string input);
        ITable Converter(StringReader input);
     
    }
}
