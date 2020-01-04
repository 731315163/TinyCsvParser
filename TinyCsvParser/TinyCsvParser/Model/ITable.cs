using System;
using TinyCsvParser.Ranges;

namespace TinyCsvParser.Model
{
    public interface ITable
    {
        
        string TableName { get; set; }
        string SheetName { get; set; }
        TableRect Rect { get; set; }
     
        TokenizedRow [] Data { get; set; }
        ITable GetTable(TableRect index);
        ArraySegment<string>[] GetRectData();
        string GetCellData(TableRect rect);

        ArraySegment<string> GetRowData(TableRect rect);
        ArraySegment<string>[] GetRectData(TableRect rect);
       
        
    }
     
}