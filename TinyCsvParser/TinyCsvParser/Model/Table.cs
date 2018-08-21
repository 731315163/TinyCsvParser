using System;
using System.Collections.Generic;
using System.Linq;

namespace TinyCsvParser.Model
{

    public class Table:ITable
    {
        public string Name;
        public string[][] Data;
        private readonly IParseIndex m_parseIndex;
        private readonly CsvParserOptions m_options;

        public Table(IEnumerable<Row> data, CsvParserOptions options, string name = null, IParseIndex parse = null )
        {
            Name = name;
            m_parseIndex = parse??DefaultParseIndex.Parse;
            m_options = options;

        }

        public void ParseData(IEnumerable<Row> csvData)
        {
            if (csvData == null)
            {
                throw new ArgumentNullException("csvData");
            }

            ParallelQuery<Row> query = csvData.Skip(m_options.SkipHeader ? 1 : 0).AsParallel();

            // If you want to get the same order as in the CSV file, this option needs to be set:
            if (m_options.KeepOrder)
            {
                query = query.AsOrdered();
            }

            query = query
                .WithDegreeOfParallelism(m_options.DegreeOfParallelism)
                .Where(row => !string.IsNullOrWhiteSpace(row.Data));

            // Ignore Lines, that start with a comment character:
            if (!string.IsNullOrWhiteSpace(m_options.CommentCharacter))
            {
                query = query.Where(line => !line.Data.StartsWith(m_options.CommentCharacter));
            }
           
            Data = new string[query.Count()][];
            query
                .Select(line => new TokenizedRow(line.Index, m_options.Tokenizer.Tokenize(line.Data)))
                .Select(fields => Data[fields.Index] = fields.Tokens);
        }
        /// <summary>
        /// ArraySegment<string> is row, [] is coloum
        /// </summary>
        /// <param name="sindex"></param>
        /// <returns></returns>
        public ArraySegment<string>[] GetData(string sindex)
        {
            int[] index = m_parseIndex.ParseIndex(sindex);
            ArraySegment<string>[] res = null;
            int w = index[2] - index[0] + 1;
            int h = index[3] - index[1] + 1;
            if (index[2] >= 0)
            {
                res = new ArraySegment<string>[h];
                for (int i = 0; i < h; ++i)
                {
                    res[i] = new ArraySegment<string>(Data[index[1] + i], index[0], w);
                }
            }
            else
            {
                res = new ArraySegment<string>[]{new ArraySegment<string>(Data[index[1]],index[0],w)};
            }
            return res;
        }

        public IEnumerable<string> ReadAll()
        {
            throw new NotImplementedException();
        }

        public int LineNum { get; }
        public IEnumerable<string> ReadLine(int index)
        {
            throw new NotImplementedException();
        }
    }

}