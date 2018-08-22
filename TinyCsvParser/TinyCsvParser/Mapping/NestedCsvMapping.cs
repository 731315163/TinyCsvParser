using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TinyCsvParser.TypeConverter;

namespace TinyCsvParser.Mapping
{
    public class NestedCsvMapping<TEntity>
        where TEntity : class, new()
    {
        private readonly ITypeConverterProvider typeConverterProvider;
        
        private readonly List<IndexToNestedPropertyMapping<TEntity>> csvPropertyMappings;

        private readonly ISerializeProvider serializeProvider;

        protected NestedCsvMapping(ISerializeProvider serializeProvider)
            : this(new TypeConverterProvider(),serializeProvider)
        { }

        protected NestedCsvMapping(ITypeConverterProvider provider,ISerializeProvider serializeProvider)
        {
            this.typeConverterProvider = provider;
            this.serializeProvider = serializeProvider;
        }
  
        protected ICsvPropertyNestedMapping<TEntity> MapProperty<TProperty>(int columnIndex, Action<TEntity,TProperty> setproperty, ISerialize<TProperty> serialize = null)
        {
            return MapProperty<TProperty>(columnIndex,setproperty,typeConverterProvider.Resolve<TProperty>(),serialize??serializeProvider.Resolve<TProperty>());
        }

        protected ICsvPropertyNestedMapping<TEntity> MapProperty<TProperty>(int columnIndex,Action<TEntity, TProperty> property, ITypeConverter<TProperty> typeConverter, ISerialize<TProperty> serialize)
        {
            if (csvPropertyMappings.Any(x => x.ColumnIndex == columnIndex))
            {
                throw new InvalidOperationException(string.Format("Duplicate mapping for column index {0}", columnIndex));
            }

            var propertyMapping = new CsvPropertyNestedMapping<TEntity, TProperty>(property, typeConverter,serialize);

            AddPropertyMapping(columnIndex, propertyMapping);

            return propertyMapping;
        }
    
        private void AddPropertyMapping<TProperty>(int columnIndex, CsvPropertyNestedMapping<TEntity, TProperty> propertyMapping)
        {
            var indexToPropertyMapping = new IndexToNestedPropertyMapping<TEntity>
            {
                ColumnIndex = columnIndex,
                PropertyMapping = propertyMapping
            };

            csvPropertyMappings.Add(indexToPropertyMapping);
        }
    }
}
