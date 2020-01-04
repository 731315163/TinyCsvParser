using TinyCsvParser.Ranges;

namespace TinyCsvParser
{
    public interface IParseAddress
    {
       
        TableRect ParseRect(string index);

        string GetTableName(string address);
        string GetSheetName(string address);

        string GetRect(string address);
    }

  
}
