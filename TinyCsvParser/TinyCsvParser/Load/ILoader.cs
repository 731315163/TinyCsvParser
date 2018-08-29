

using System;
using System.Collections.Generic;
using TinyCsvParser.Model;

namespace TinyCsvParser.Load
{
     public interface ILoader
    {
        IEnumerable<Row> Load(Tuple<string,string> key);
    }
}
