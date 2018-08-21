using System.Collections.Generic;

namespace TinyCsvParser.Model
{
    public interface ITable
    {
        IEnumerable<string> ReadAll();
        int LineNum { get; }
        IEnumerable<string> ReadLine(int index);
    }
}
