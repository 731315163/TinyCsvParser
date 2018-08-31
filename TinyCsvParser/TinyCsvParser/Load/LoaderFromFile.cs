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
        protected Encoding encoding = Encoding.UTF8;
        protected Func<Tuple<string, string>,string> getpath;

        public LoaderFromFile(Func<Tuple<string,string>,string> getpath,Encoding encoding = null)
        {
            this.getpath = getpath;
            this.encoding = encoding;
        }

        public IEnumerable<Row> Load(Tuple<string,string> key)
        {
            string fileName = getpath(key);
            if (fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }

            return File
                .ReadLines(fileName, encoding)
                .Select((line, index) => new Row(index, line));

        }
    }
}
