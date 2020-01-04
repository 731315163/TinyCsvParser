using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TinyCsvParser.Model;

namespace TinyCsvParser.Load
{
    public class LoaderFromString:ILoader
    {
        protected CsvReaderOptions csvReaderOptions;
        protected Func<string, string, string> getdata;

        public LoaderFromString(Func<string, string, string> getdata)
        {
            this.getdata = getdata;
        }
      

      

        public IEnumerable<Row> Load(string TableName, string SheetName)
        {
            string csvData = getdata(TableName,SheetName);
           
            return csvData
                .Split(csvReaderOptions.NewLine, StringSplitOptions.None)
                .Select((line, index) => new Row(index, line));
        }
    }
}
