using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TinyCsvParser.Ranges;

namespace TinyCsvParser.Model
{
    /// <summary>
    /// 保存一个csv的字符串数据
    /// </summary>
    public class Table : ITable
    {
       
        public string TableName { get; set; }
        public string SheetName { get; set; }
        public TableRect Rect { get; set; }
      
        public TokenizedRow[] Data { get; set; }

        public Table( string TableName, string SheetName, TableRect rect = new TableRect())
        {
            this.TableName = TableName;
            this.SheetName = SheetName;
            this.Rect = rect;
        }

        public Table(TokenizedRow[] data, string TableName, string SheetName, TableRect rect = new TableRect())
        {
            this.Data = data;
            this.TableName = TableName;
            this.SheetName = SheetName;
            this.Rect = rect;
        }

        public ITable GetTable(TableRect index)
        {
            return new Table(this.Data, this.TableName, this.SheetName, index);
        }
        public ArraySegment<string>[] GetRectData()
        {
            if (Rect.Area == 0)
            {
              var res = new ArraySegment<string>[Data.Length];
                for (int i = 0; i < Data.Length; ++i)
                {
                    res[i] = new ArraySegment<string>(Data[i].Tokens, 0, Data[i].Tokens.Length);
                }
                return res;
            }
            return GetRectData(Rect);
        }
        /// <summary>
        /// ArraySegment<string> is row, [] is coloum
        /// </summary>
        /// <param name="sindex"></param>
        /// <returns></returns>
        ///   public ArraySegment<string>[] GetRectData()
        public ArraySegment<string>[] GetRectData(TableRect index)
        {
            ArraySegment<string>[] res = null;
            if (index.heigh >= 0)
            {
                res = new ArraySegment<string>[index.heigh];
                for (int i = 0; i < index.heigh; ++i)
                {
                    res[i] = new ArraySegment<string>(Data[index.y + i].Tokens, index.x, index.width);
                }
            }
            else
            {
                res = new[] { new ArraySegment<string>(Data[index.y].Tokens, index.x, index.width) };
            }
            return res;
        }

        public string GetCellData(TableRect rect)
        {
            return 
                Data[rect.y].Tokens[rect.x];
        }

        public ArraySegment<string> GetRowData(TableRect rect)
        {
            return new ArraySegment<string>(Data[rect.y].Tokens,rect.x,rect.width);
        }
    }
 



}