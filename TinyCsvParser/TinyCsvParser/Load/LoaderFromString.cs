using System;
using System.Collections.Generic;
using System.Linq;
using TinyCsvParser.Model;

namespace TinyCsvParser.Load
{
    public class LoaderFromString:ILoader
    {
        protected CsvReaderOptions csvReaderOptions;
        protected Func<Tuple<string, string>, string> getdata;

        public LoaderFromString(Func<Tuple<string, string>, string> getdata, CsvParserOptions csvParserOptions)
        {
            this.getdata = getdata;
        }
      

        public IEnumerable<Row> Load(Tuple<string, string> key)
        {
            string csvData =getdata(key);
            return csvData
                .Split(csvReaderOptions.NewLine, StringSplitOptions.None)
                .Select((line, index) => new Row(index, line));
        }
    }
}
