using System;

namespace TinyCsvParser.Model
{

    public class Table
    {
        public string Name;
        public readonly string[,] Data;
        private readonly IParseIndex parseIndex;

        public Table(string[,] data, string name = null,IParseIndex parse = null)
        {
            Data = data;
            parseIndex = parse??DefaultParseIndex.Parse;
            Name = name;
        }
        public string[,] GetData(string sindex)
        {
            int[] index = parseIndex.ParseIndex(sindex);
            if (index[2] >= 0)
            {
                int w =index[2] - index[0] + 1;
                int h = index[3] - index[1] + 1;
                string[,] res = new string[w,h];
                for (int i = index[0] , x = 0; i < w; i++,x++)
                {
                    for (int j = index[1] , y = 0; j < h; j++, y++)
                    {
                        res[x, y] = Data[i, j];
                    }
                }
                return res;
            }
            else
            {
                string[,] res = new string[1,1];
                res[0, 0] = Data[index[0], index[1]];
                return res;
            }
           
        }
    }

}