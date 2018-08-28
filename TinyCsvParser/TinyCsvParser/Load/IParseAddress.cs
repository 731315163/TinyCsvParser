
namespace TinyCsvParser.Load
{
    public interface IParseAddress
    {
        /// <summary>
        /// 0 = xstart ,1 = ystart ,2 = xend,3 = yend,rect data
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        int[] ParseIndex(string index);

        string GetTableName(string address);
        string GetSheetName(string address);

        string GetIndex(string address);
    }

  
}
