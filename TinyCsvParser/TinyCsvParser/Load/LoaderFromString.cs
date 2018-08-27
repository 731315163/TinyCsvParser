using System;
using System.Collections.Generic;
using System.Linq;
using TinyCsvParser.Model;

namespace TinyCsvParser.Load
{
    public class LoaderFromString:ILoader
    {
        protected CsvReaderOptions csvReaderOptions;
        public virtual string KeyToString(string key)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Row> Load(string key)
        {
            string csvData = KeyToString(key);
            return csvData
                .Split(csvReaderOptions.NewLine, StringSplitOptions.None)
                .Select((line, index) => new Row(index, line));
        }
    }
}
