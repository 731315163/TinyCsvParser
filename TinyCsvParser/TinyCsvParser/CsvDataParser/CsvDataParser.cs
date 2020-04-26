using System;
using System.Collections.Generic;
using System.Linq;
using TinyCsvParser.Model;

namespace TinyCsvParser
{
    public class CsvDataParser 
    {
        private readonly CsvParserOptions options;
       


        public CsvDataParser(CsvParserOptions options)
        {
            this.options = options;
         
        }
        public void Parse(string csvData,ref ITable table,CsvReaderOptions csvReaderOptions)
        {
            
            var lines = csvData
               .Split(csvReaderOptions.NewLine, StringSplitOptions.None)
               .Select((line, index) => new Row(index, line));
            Parse(lines,ref table);
        }

        public void Parse(IEnumerable<Row> csvData,ref ITable table)
        {
            if (csvData == null)
            {
                throw new ArgumentNullException(nameof(csvData));
            }

            ParallelQuery<Row> query = csvData
                .Skip(options.SkipHeader ? 1 : 0)
                .AsParallel();

            // If you want to get the same order as in the CSV file, this option needs to be set:
            if (options.KeepOrder)
            {
                query = query.AsOrdered();
            }

            query = query
                .WithDegreeOfParallelism(options.DegreeOfParallelism)
                .Where(row => !string.IsNullOrWhiteSpace(row.Data));

            // Ignore Lines, that start with a comment character:
            if (!string.IsNullOrWhiteSpace(options.CommentCharacter))
            {
                query = query.Where(line => !line.Data.StartsWith(options.CommentCharacter));
            }

         table.Data = query
           .Select(line => new TokenizedRow(line.Index, options.Tokenizer.Tokenize(line.Data)))
           .ToArray();
            

        }

        public override string ToString()
        {
            return $"CsvParser (Options = {options})";
        }
    }
}
