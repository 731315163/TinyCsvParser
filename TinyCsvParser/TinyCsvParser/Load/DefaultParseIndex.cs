using System;

namespace TinyCsvParser.Load
{
    public class  DefaultParseIndex:IParseIndex
    {
        public static DefaultParseIndex Parse = new DefaultParseIndex();
        public int[] ParseIndex(string index)
        {
            var rect = new int[4];
            int i = 0;
            int x = 0;
            char prec = 'A';
            for (var j = 0; j < index.Length; j++)
            {
                if( ! char.IsLetterOrDigit(index[j]))
                    continue;
                bool precisDigit = char.IsDigit(prec);
                if (char.IsDigit(index[j]))
                {
                    if (precisDigit)
                    {
                        x = x * 10 + (index[j] - '0');
                    }
                    else
                    {
                        rect[i] = x;
                        x = index[j] - '0';
                        prec = index[j];
                        ++i;
                    }
                }
                else
                {
                    //char is letter
                    if (precisDigit)
                    {
                        rect[i] = x;
                        x =( index[j] - 'A') + 1;
                        prec = index[j];
                        ++i;
                    }
                    else
                    {
                        x = x * 26 + (index[j] - 'A'+1);
                    }
                }
                if (j == index.Length - 1)
                    rect[i] = x;
            }
            CheckArrayException(rect);

            return rect;

        }

        protected void CheckArrayException(int[] array)
        {
            for (var i = 0; i < array.Length; i++)
            {
                array[i] -= 1;
            }
            CheckException(array[0],array[1]);
            CheckException(array[2],array[3]);
            if( (array[0] < 0 || array[1] < 0) && (array[2] >= 0 || array[3] >= 0))
                throw new IndexOutOfRangeException();
        }
        protected void CheckException(int a, int b)
        {
            if ((a < 0 && b >=0) || (a >= 0 && b < 0))
                throw new IndexOutOfRangeException();
        }
    }
}