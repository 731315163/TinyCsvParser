using System;
using System.Collections.Generic;

namespace TinyCsvParser.Model
{
    public interface ITable
    {
        /// <summary>
        /// one is table name,  two is sheet name
        /// </summary>
        Tuple<string,string> Key { get; set; }
        IEnumerable<IEnumerable<string>> ReadAllLines();
        int LineCount { get; }
        IEnumerable<string> ReadLine(int index);

        ITable CreateTable(string index);
    }
}
