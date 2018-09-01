using System;
using System.Collections.Generic;
using System.Linq;

namespace TinyCsvParser.Model
{
    /// <summary>
    /// 保存一个csv的字符串数据
    /// </summary>
    public class Table:ITable
    {
        private readonly CsvParserOptions m_options;

        public Table(IEnumerable<Row> data, CsvParserOptions options,Tuple<string,string> key)
        {
            m_options = options;
            Data = ParseData(data);
            this.Key = key;
        }

        public Table(ArraySegment<string>[] data,Tuple<string, string> key)
        {
            this.Key = key;
            this.Data = data;
        }
        public ArraySegment<string>[] ParseData(IEnumerable<Row> csvData)
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
           
            var data = new ArraySegment<string>[query.Count()];
            query
                .Select(line => new TokenizedRow(line.Index, m_options.Tokenizer.Tokenize(line.Data)))
                .Select(fields => data[fields.Index] = new ArraySegment<string>(fields.Tokens));
            return data;
        }

        public Tuple<string, string> Key { get; set; }
        public ArraySegment<string>[] Data { get; set; }

        public IEnumerable<IEnumerable<string>> ReadAllCell()
        {
            for (int i = 0; i < Data.Length; ++i)
            {
                yield return ReadLine(i);
            }
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

        public ITable CreateTable(int[] index)
        {
            var table = new Table(GetData(index),this.Key);
            return table;
        }
      
        /// <summary>
        /// ArraySegment<string> is row, [] is coloum
        /// </summary>
        /// <param name="sindex"></param>
        /// <returns></returns>
        public ArraySegment<string>[] GetData(int[] index)
        {
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
                res = new[] { new ArraySegment<string>(Data[index[1]].Array, index[0], w) };
            }
            return res;
        }
    }

}