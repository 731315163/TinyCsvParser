using System;
using System.Collections.Generic;
using System.Linq;
using TinyCsvParser.Mapping;
using TinyCsvParser.Model;

namespace TinyCsvParser
{
    public class NestedCsvParser<TEntity> : ICsvParser<TEntity>
        where TEntity : class, new()
    {
        public ParallelQuery<CsvMappingResult<TEntity>> Parse(IEnumerable<Row> csvData)
        {
            return null;
        }
    }
}
