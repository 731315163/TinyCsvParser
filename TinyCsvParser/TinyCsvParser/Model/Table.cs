using System;
using System.Collections.Generic;
using System.Linq;
using TinyCsvParser.Tokenizer;

namespace TinyCsvParser.Model
{

    public class Table
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

            ParallelQuery<Row> query = csvData
                .Skip(m_options.SkipHeader ? 1 : 0)
                .AsParallel();

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

        public string[,] GetData(string sindex)
        {
            int[] index = m_parseIndex.ParseIndex(sindex);
            if (index[2] >= 0)
            {
                int w =index[2] - index[0] + 1;
                int h = index[3] - index[1] + 1;
                string[,] res = new string[w,h];
                for (int i = index[0] , x = 0; i < w; i++,x++)
                {
                    for (int j = index[1] , y = 0; j < h; j++, y++)
                    {
                        res[x,y] = Data[i][j];
                    }
                }
                return res;
            }
            else
            {
                string[,] res = new string[1,1];
                res[0, 0] = Data[index[0]][index[1]];
                return res;
            }
           
        }
    }

}