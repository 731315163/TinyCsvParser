using System.Collections.Generic;

namespace TinyCsvParser.Model
{
    public interface ITable
    {
        IEnumerable<string> ReadAll();
        int LineCount { get; }
        IEnumerable<string> ReadLine(int index);
    }
}
