
using System;
using System.Collections.Generic;
using TinyCsvParser.Load;

namespace TinyCsvParser.Model
{
    public class TableContext
    {
        protected   IDictionary<Tuple<string,string>, ITable> context = new Dictionary<Tuple<string, string>, ITable>();
        protected IParseAddress parseAddress;
        protected  ILoader loader;
        public static TableContext Instance;

        public ITable GetTable(string key, ITable table = null)
        {
            var tablename = parseAddress.GetTableName(key);
            var sheetname = parseAddress.GetSheetName(key);
            var dataindex = parseAddress.GetIndex(key);
            Tuple<string, string> tablekey = new Tuple<string, string>(string.IsNullOrEmpty(tablename) ? table.Key.Item1 : tablename, string.IsNullOrEmpty(sheetname) ? table.Key.Item2 : sheetname);
            ITable rettable;
            if( ! context.TryGetValue(tablekey,out rettable))
            {
                rettable = AddTable(tablekey, loader.Load(tablekey));
            }

            if (string.IsNullOrEmpty(dataindex))
                return rettable;
            return rettable.CreateTable(dataindex);
        }

        public ITable AddTable(Tuple<string, string> key, IEnumerable<Row> rows)
        {
            //todo
            throw new NotImplementedException();
        }
        
    }
}