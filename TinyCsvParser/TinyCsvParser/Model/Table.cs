using System;
using System.Collections.Generic;
using System.Linq;
using TinyCsvParser.Load;

namespace TinyCsvParser.Model
{
    /// <summary>
    /// 保存一个csv的字符串数据
    /// </summary>
    public class Table:ITable
    {
        public string Name;
        public ArraySegment<string>[] Data;
        private readonly IParseIndex m_parseIndex;
        private readonly CsvParserOptions m_options;

        public Table(IEnumerable<Row> data, CsvParserOptions options, string name, IParseIndex parse = null)
        {
            Name = name;
            m_parseIndex = parse??DefaultParseIndex.Parse;
            m_options = options;
        }

        public Table(ArraySegment<string>[] data)
        {
            this.Data = data;
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
           
            Data = new ArraySegment<string>[query.Count()];
            query
                .Select(line => new TokenizedRow(line.Index, m_options.Tokenizer.Tokenize(line.Data)))
                .Select(fields => Data[fields.Index] = new ArraySegment<string>(fields.Tokens));
        }

        public ITable GetTable(string sindex)
        {
            var table = new Table(GetData(sindex));
            return table;
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
                    res[i] = new ArraySegment<string>(Data[index[1] + i].Array, index[0], w);
                }
            }
            else
            {
                res = new[]{new ArraySegment<string>(Data[index[1]].Array,index[0],w)};
            }
            return res;
        }

        public IEnumerable<string> ReadAll()
        {
            throw new NotImplementedException();
        }

        public int LineCount
        {
            get { return Data.Length; }
        }

        public IEnumerable<string> ReadLine(int index)
        {
            var line = Data[index];
            int count = line.Count + line.Offset;
            for (var i = line.Offset; i < count; i++)
            {
                yield return line.Array[i];
            }
        }
    }

}