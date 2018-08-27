
using System.Collections.Generic;
using TinyCsvParser.Load;

namespace TinyCsvParser.Model
{
    public class TableContext
    {
        protected IDictionary<string, ITable> context = new Dictionary<string, ITable>();
        protected ILoader loader;
        public ITable GetTable(string tablename,string sheetname,string keyorrect)
        {
            ITable res;
            if (context.TryGetValue(keyorrect, out res))
                return res;
            var csvdata = loader.Load(keyorrect);
            //todo
            res = new Table(csvdata,null,"");
            context.Add(keyorrect,res);
            return res;
        }
    }
}
