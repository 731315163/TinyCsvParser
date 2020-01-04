namespace TinyCsvParser.Ranges
{
    /// <summary>
    /// 原点在左下角
    /// </summary>
    public struct TableRect
    {
        public int x, y, width, heigh;
       
        public int Area
        {
            get 
            {
                return width * heigh;
            }
        }
        /// <summary>
        /// 以 this Table Rect 为参考系的绝对 坐标
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public TableRect GetRelativeCoordinateRect(TableRect rect)
        {
            int isnull = rect.Area;
            int w = isnull == 0 ? width:rect.width;
            int h = isnull == 0 ? heigh:rect.heigh;
            return new TableRect(x + rect.x, y + rect.y, w, h);
        }
        public TableRect(int x, int y) :this(x, y,1,1)
        {

        }
        public TableRect(int x, int y,int w,int h)
        {
            this.x = x;
            this.y = y;
            this.width = w;
            this.heigh = h;
           
        }
      

        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(TableRect))
                return false;
            var rect = (TableRect)obj;
            if (this.x == rect.x && this.y == rect.y && this.width == rect.width && this.heigh == rect.heigh)
                return true;
            else return false;

        }
    }
}
