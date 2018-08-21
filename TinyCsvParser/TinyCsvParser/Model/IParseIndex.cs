using System;

namespace TinyCsvParser.Model
{
    public interface IParseIndex
    {
        /// <summary>
        /// 0 = xstart ,1 = ystart ,2 = xend,3 = yend
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        int[] ParseIndex(string index);
    }

  
}
