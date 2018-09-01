using System;
using System.Collections.Generic;

namespace TinyCsvParser.Model
{
    /// <summary>
    /// table 容器类
    /// </summary>
    public interface ITableContext
    {
         ITable GetTable(string key, ITable table);

        ITable GetTable(Tuple<string, string> key);


        ITable AddTable(Tuple<string, string> key, IEnumerable<Row> rows);

    }
}
