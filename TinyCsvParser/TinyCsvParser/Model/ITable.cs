using System;
using System.Collections.Generic;

namespace TinyCsvParser.Model
{
    public interface ITable
    {
        Tuple<string,string> Key { get; set; }
        IEnumerable<IEnumerable<string>> ReadAllLines();
        int LineCount { get; }
        IEnumerable<string> ReadLine(int index);
    }
}
