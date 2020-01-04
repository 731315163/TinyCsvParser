using System;
using TinyCsvParser.Model;
using TinyCsvParser.Ranges;
using TinyCsvParser.TypeConverter;

namespace TinyCsvParser.Mapping
{
    public class NestedBaseTypePropertyMapping<TEntity,TProperty> : INestedPropertyMapping<TEntity,ITable,TableRect>
    {
        private string propertyName;
        private ITypeConverter<TProperty> propertyConverter;
        private Action<TEntity, TProperty> propertySetter;
        public TableRect Rect { get; set; }

    

       
        public NestedBaseTypePropertyMapping(TableRect range, Action<TEntity, TProperty> property, ITypeConverter<TProperty> typeConverter)
        {
            propertySetter = property;
            propertyConverter = typeConverter;
            Rect= range;
        }

        public bool TryMapValue(TEntity entity,  ITable value,TableRect rect)
        {
            if (Rect.Area > 1)
                return false;
            TableRect key = value.Rect.GetRelativeCoordinateRect(rect).GetRelativeCoordinateRect(Rect);
            if (!propertyConverter.TryConvert(value.GetCellData(key), out TProperty convertedValue))
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