using System;
using System.Text;
using System.Text.RegularExpressions;
using TinyCsvParser.Ranges;

namespace TinyCsvParser
{
    public class  ParseAddress:IParseAddress
    {
        private ParseAddress() { }
        public static IParseAddress Instance
        {
            get { return Singleton.instance; }
        }
        public virtual TableRect ParseRect(string index)
        {
          
            var coordinate =GetRect(index).Split(':');
            if(coordinate.Length <=0 || coordinate.Length > 2)
            {
                throw new Exception("The coordinates are wrong");
            }
            else
            {
                var xy = GetCoordinate(coordinate[0]);
                var tablerect = new TableRect(xy.Item1 - 1, xy.Item2 - 1);
                if (coordinate.Length == 2)
                {
                    var wz = GetCoordinate(coordinate[1]);
                    tablerect.width = wz.Item1 - xy.Item1 + 1;
                    tablerect.heigh = wz.Item2 - xy.Item2 + 1;
                }
                return tablerect;
            }

        }
        /// <summary>
        ///  坐标系以{1,1} 为原点
        ///  The coordinate system takes {1,1} as the origin
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected Tuple<int,int> GetCoordinate(string index)
        {
            int x = 0;
            int y = 0;
            for (int i = 0; i < index.Length; i++)
            {
                char c = index[i];
                if (!Char.IsLetterOrDigit(c))
                    continue;
                if (char.IsLetter(c))
                {
                    x = 26 * x + c - 'A'+1;
                }
                else
                {
                    y = 10 * y + c - '0';
                }
            }
            return new Tuple<int, int>(x, y);
        }
        protected void CheckArrayException(int[] array)
        {
            for (var i = 0; i < array.Length; i++)
            {
                array[i] -= 1;
            }
            CheckNumException(array[0],array[1]);
            CheckNumException(array[2],array[3]);
            if( (array[0] < 0 || array[1] < 0) && (array[2] >= 0 || array[3] >= 0))
                throw new IndexOutOfRangeException();
        }
        protected void CheckNumException(int a, int b)
        {
            if ((a < 0 && b >=0) || (a >= 0 && b < 0))
                throw new IndexOutOfRangeException();
        }

        public virtual string GetTableName(string address)
        {
            Match res = Regex.Match(address, @"^\[.+\]");
            return CopyMathchString(res, 1, res.Value.Length - 1);
        }

        public virtual string GetSheetName(string address)
        {
            Match res = Regex.Match(address, @"\].+!");
            return CopyMathchString(res, 1, res.Value.Length - 1);
        }

        public virtual string GetRect(string address)
        {
            Match res = Regex.Match(address, "!?(([A-Z]+[0-9]+:?){1,2})");
            if (res.Success)
            {
                if(res.Value[0] == '!')
                    return CopyMathchString(res, 1, res.Value.Length);
                else
                    return res.Value;
            }
            return String.Empty;
        }

        protected string CopyMathchString(Match m, int offset, int count)
        {
           
            if ( ! m.Success)
                return string.Empty;
            char[] path = new char[count-offset];
            m.Value.CopyTo(offset, path, 0, count-offset);
            return new string(path);
                  }

        private class Singleton
        {
            internal static readonly IParseAddress instance = null;
            static Singleton()
            {
                instance = new ParseAddress();
            }
        }

    }
}