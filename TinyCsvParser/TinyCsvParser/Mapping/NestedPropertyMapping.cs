using System;
using TinyCsvParser.Model;
using TinyCsvParser.NestedTypeConverter;
using TinyCsvParser.Ranges;


namespace TinyCsvParser.Mapping
{
    public class NestedPropertyMapping<TEntity, TProperty> : INestedPropertyMapping<TEntity, ITable,TableRect>
    {
        private string propertyName;
        private INestedTypeConverter<TProperty> propertyConverter;
        private Action<TEntity, TProperty> propertySetter;
        //4
        public TableRect Rect { get; set; }


        public NestedPropertyMapping(TableRect range, Action<TEntity, TProperty> property, INestedTypeConverter<TProperty> typeConverter)
        {
            propertySetter = property;
            propertyConverter = typeConverter;
            Rect = range;
        }
        /// <summary>
        /// 嵌套类，不加table坐标
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="value"></param>
        /// <param name="rect">坐标系原点</param>
        /// <returns></returns>        
        public bool TryMapValue(TEntity entity, ITable value,TableRect rect)
        {
            var relrect = rect.GetRelativeCoordinateRect(Rect);
            if (!propertyConverter.TryConvert(relrect,value, out TProperty convertedValue))
            {
                return false;
            }
            propertySetter(entity, convertedValue);

            return true;
        }

        public override string ToString()
        {
            return string.Format("CsvPropertyMapping (PropertyName = {0}, Converter = {1})", propertyName, propertyConverter);
        }
    }
}