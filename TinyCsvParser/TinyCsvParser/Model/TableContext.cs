using System;
using System.Collections.Generic;
using TinyCsvParser.Load;

namespace TinyCsvParser.Model
{
    public class TableContext:ITableContext
    {
        protected   IDictionary<Tuple<string,string>, ITable> context = new Dictionary<Tuple<string, string>, ITable>();
        protected IParseAddress parseAddress;
        protected  ILoader loader;
        public static TableContext Instance;
        public CsvParserOptions csvParserOptions;

        public ITable GetTable(string key, ITable table = null)
        {
            var tablename = parseAddress.GetTableName(key);
            var sheetname = parseAddress.GetSheetName(key);
            var dataindex = parseAddress.GetIndex(key);
            tablename = string.IsNullOrEmpty(tablename) ? table.Key.Item1 : tablename;
            sheetname = string.IsNullOrEmpty(sheetname) ? table.Key.Item2 : sheetname;
            Tuple<string, string> tablekey = new Tuple<string, string>( tablename, sheetname);
            ITable rettable = GetTable(tablekey);
            if (string.IsNullOrEmpty(dataindex))
                return rettable;
            return rettable.CreateTable(parseAddress.ParseIndex(dataindex));
        }

        public ITable GetTable(Tuple<string, string> tablekey)
        {
            ITable rettable;
            if (!context.TryGetValue(tablekey, out rettable))
            {
                rettable = AddTable(tablekey, loader.Load(tablekey));
            }
            return rettable;
        }

        public ITable AddTable(Tuple<string, string> key, IEnumerable<Row> rows)
        {
           var table = new Table(rows, csvParserOptions,key);
            this.context.Add(key,table);
            return table;
        }
        
    }
}