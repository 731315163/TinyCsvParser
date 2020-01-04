using System;
using System.Collections.Generic;

namespace TinyCsvParser.Model
{
    /// <summary>
    /// table 容器类
    /// </summary>
    public interface ITableTree
    {
     

        ITable GetTable(params string[] path);

        void AddTable( ITable table, params string[] path);

        bool ContainTable(params string[] path);

    }
}
