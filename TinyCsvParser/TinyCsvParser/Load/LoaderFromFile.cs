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
        protected Encoding encoding;

        public virtual string KeyToPath(string key)
        {
            return null;
        }

        public IEnumerable<Row> Load(string key)
        {
            string fileName = KeyToPath(key);
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
