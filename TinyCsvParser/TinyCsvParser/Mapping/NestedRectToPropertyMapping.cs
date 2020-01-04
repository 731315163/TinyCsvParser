
using TinyCsvParser.Model;
using TinyCsvParser.Ranges;

namespace TinyCsvParser.Mapping
{
    public class NestedRectToPropertyMapping<TEntity>
    {
        public TableRect Rect { get; set; }

        public INestedPropertyMapping<TEntity, ITable,TableRect> PropertyMapping { get; set; }

        public override string ToString()
        {
            return $"IndexToPropertyMapping (Range = {Rect}, PropertyMapping = {PropertyMapping}";
        }
    }
    public class NestedRectToConverterMapping<TTargetType>
    {
        public TableRect Rect { get; set; }

        public IConverterMapping<TTargetType> PropertyMapping { get; set; }

        public override string ToString()
        {
            return $"IndexToPropertyMapping (Range = {Rect}, PropertyMapping = {PropertyMapping}";
        }
    }

}
