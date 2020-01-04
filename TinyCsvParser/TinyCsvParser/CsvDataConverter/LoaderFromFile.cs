using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TinyCsvParser.Model;

namespace TinyCsvParser.Load
{
    public class LoaderFromFile:ILoader
    {
        protected Encoding encoding ;
        protected Func<string, string,string> getpath;
        protected CsvReaderOptions readerOptions;

        public LoaderFromFile(Func<string,string,string> getpath,CsvReaderOptions option)
        {
           
            this.getpath = getpath;
            this.readerOptions = option;
           
            
        }


        public IEnumerable<Row> Load(string TableName, string SheetName)
        {
            string fileName = getpath(TableName,SheetName);
            if (fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }

            return File
                .ReadAllText(fileName)
                .Split(readerOptions.NewLine, StringSplitOptions.None)
                .Select((line, index) => new Row(index, line));
        }
    }
}
